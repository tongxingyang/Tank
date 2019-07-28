---------------------------------------------
--- InputMgr
--- Created by thrt520.
--- DateTime: 2018/7/24
---------------------------------------------
---@class InputMgr
local InputMgr = {}
local this = InputMgr

local cInputMgr = InputManager.Instance


local MSGClickEsc  =require("Game.Event.Message.Input.MSGClickEsc")
local MSGClickRightMouseBtn  =require("Game.Event.Message.Input.MSGClickRightMouseBtn")
local MSGClickLeftMouseBtn  =require("Game.Event.Message.Input.MSGClickLeftMouseBtn")


function InputMgr.Startup()
	cInputMgr:AddOnEscListener(this._onClickEsc)
	cInputMgr:AddOnRightMouseClickListener(this._onClickRightMouseBtn)
	cInputMgr:AddOnLeftMouseClickListener(this._onClickLeftMouse)
end

function InputMgr.OnPrepareScene()

end

function InputMgr.OnIntoScene()

end

function InputMgr.OnLeaveScene()

end

function InputMgr._onClickEsc()
	EventBus:Brocast(MSGClickEsc.new())
end

function InputMgr._onClickRightMouseBtn()
	EventBus:Brocast(MSGClickRightMouseBtn.new())
end

function InputMgr._onClickLeftMouse()
	EventBus:Brocast(MSGClickLeftMouseBtn.new())
end

return InputMgr