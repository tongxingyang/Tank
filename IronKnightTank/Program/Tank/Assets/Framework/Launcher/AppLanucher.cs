using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

using Object = UnityEngine.Object;
namespace XQFramework.Laucher
{
    /// <summary>
    /// 应用启动入口
    /// </summary>
    public class AppLanucher
    {
        /// <summary>
        /// 已经成功启动
        /// </summary>
        public bool IsLanuchComplete = false;

        /// <summary>
        /// 进度更新回调
        /// </summary>
        private Action<float, string> onProgress;

        /// <summary>
        /// 完成回调
        /// </summary>
        private Action onFinish;

        /// <summary>
        /// 当前正在运行的任务序号
        /// </summary>
        private int currTaskIndex = -1;

        /// <summary>
        /// 进度总长
        /// </summary>
        private float totalWeight;

        /// <summary>
        /// 已完成进度
        /// </summary>
        private float finishWeight;

        private List<ILanucherTask> tasks = new List<ILanucherTask>();

        /// <summary>
        /// 添加一个启动任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void AddTask<T>() where T : ILanucherTask
        {
            this.tasks.Add(this.CreateTask(typeof(T)));
        }

        /// <summary>
        /// 添加一个启动任务
        /// </summary>
        public void AddTask(ILanucherTask lanucherTask)
        {
            lanucherTask.SetTaskProgress = this.OnProgress;
            this.tasks.Add(lanucherTask);
        }

        public void AddProgressListener(Action<float, string> onProgress)
        {
            this.onProgress = onProgress;
        }

        public void AddFinishListener(Action onFinish)
        {
            this.onFinish = onFinish;
        }

        public void Lanuch()
        {
            this.totalWeight = 0;
            this.currTaskIndex = -1;
            this.finishWeight = 0;

            for (int i = 0; i < this.tasks.Count; i++)
            {
                var gameEntryTask = this.tasks[i];
                this.totalWeight += gameEntryTask.Weight;
            }

            this.NextTask();
        }

        private void NextTask()
        {
            if (this.currTaskIndex >= 0)
            {
                var finishedTask = this.tasks[this.currTaskIndex];
                this.finishWeight += finishedTask.Weight;
            }
            this.currTaskIndex++;
            if (this.tasks.Count > this.currTaskIndex)
            {
                var currTask = this.tasks[this.currTaskIndex];
                currTask.StartTask();
            }
            else
            {
                this.Finish();
            }
        }

        private void OnProgress(ILanucherTask task, float progress, string message)
        {
            var currTask = this.tasks[this.currTaskIndex];
            if (currTask != task)
            {
                return;
            }
            var currFinished = (float)currTask.Weight * progress + this.finishWeight;

            if (this.onProgress != null)
            {
                this.onProgress(currFinished / this.totalWeight, message);
            }

            if (progress >= 1)
            {
                this.NextTask();
            }
        }

        private void Finish()
        {
            this.IsLanuchComplete = true;
            if (this.onFinish != null)
            {
                this.onFinish();
            }
        }

        private ILanucherTask CreateTask(Type taskType)
        {
            ILanucherTask task = null;
            if (taskType.IsAssignableFrom(typeof(MonoBehaviour)))
            {
                task = Object.FindObjectOfType(taskType) as ILanucherTask;
                if (task == null)
                {
                    var taskObject = new GameObject(taskType.Name);
                    task = taskObject.AddComponent(taskType) as ILanucherTask;
                }
            }
            else
            {
                ConstructorInfo constructorInfo = taskType.GetConstructor(Type.EmptyTypes);
                task = constructorInfo.Invoke(null) as ILanucherTask; ;
            }

            task.SetTaskProgress = this.OnProgress;
            return task;
        }
    }
}