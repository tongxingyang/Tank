---------------------------------------------
--- JsonDataLoader
--- Created by thrt520.
--- DateTime: 2018/5/21
---------------------------------------------

local JsonDataLoader = {}
JsonDataLoader.Proxy = nil
JsonDataLoader.Weight = 5

require("Game.JsonTool.JsonDataMgr")

function JsonDataLoader.StartTask()
	JsonDataMgr.PreLoadData(JsonDataLoader._progress)
end

function JsonDataLoader._progress(progress)
	JsonDataLoader.Proxy:SetProgress(progress , "加载配置文件")
end

return JsonDataLoader