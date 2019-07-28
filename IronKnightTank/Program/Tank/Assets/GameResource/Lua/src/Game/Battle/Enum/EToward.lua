---------------------------------------------
--- EFaceToward
--- Created by thrt520.
--- DateTime: 2018/5/15
---------------------------------------------
local Vector3 = require("UnityEngine.Vector3")
---@class EToward
EToward = {
	Right = 1 ,
	Left =2 ,
	Up = 3 ,
	Down = 4,
	RightUp = 5,
	RightDown = 6,
	LeftUp = 7,
	LeftDown = 8,
}
local etowardVectorCache = {}
etowardVectorCache[EToward.Right] = Vector3.New(0 , 90, 0)
etowardVectorCache[EToward.Left] = Vector3.New(0 , -90, 0)
etowardVectorCache[EToward.Up] = Vector3.New(0 , 0, 0)
etowardVectorCache[EToward.Down] = Vector3.New(0 , 180, 0)
etowardVectorCache[EToward.LeftUp] = Vector3.New(0 , -45, 0)
etowardVectorCache[EToward.LeftDown] = Vector3.New(0 , -135, 0)
etowardVectorCache[EToward.RightUp] = Vector3.New(0 , 45, 0)
etowardVectorCache[EToward.RightDown] = Vector3.New(0 , 135, 0)

etowardVectorCache = setmetatable(etowardVectorCache , {__index = function ()
	return Vector3.New( 0 , 0, 0)
end})

EToward.GetAngle = function(towrad)
	return etowardVectorCache[towrad]
end

local etowardGridPosCache = {}
etowardGridPosCache[EToward.Right] =  GridPos.New(1 , 0)
etowardGridPosCache[EToward.Left] = GridPos.New(-1 , 0)
etowardGridPosCache[EToward.Up] = GridPos.New(0 , 1)
etowardGridPosCache[EToward.Down] = GridPos.New(0 , -1)
etowardGridPosCache[EToward.LeftUp] = GridPos.New(-1 , 1)
etowardGridPosCache[EToward.LeftDown] = GridPos.New(-1 , -1)
etowardGridPosCache[EToward.RightUp] = GridPos.New(1 , 1)
etowardGridPosCache[EToward.RightDown] = GridPos.New(1 , -1)

etowardGridPosCache = setmetatable(etowardGridPosCache , {__index = function ()
	return GridPos.New(0 , 0)
end})

EToward.GetVector = function(towrad)
	return etowardGridPosCache[towrad]
end

EToward.Vector2Enum = function(vec)
	if not vec then
		clog("EFaceToward.Vector2Enum vec nil wrong")
		return nil
	end
	local x , y = vec.x , vec.y

	if x ==1 and y == 0 then
		return EToward.Right
	elseif x ==1 and y == 1 then
		return EToward.RightUp
	elseif x ==1 and y == -1 then
		return EToward.RightDown
	elseif x == 0 and y == 1 then
		return EToward.Up
	elseif x == 0 and y == -1 then
		return EToward.Down
	elseif x == -1 and y == 0 then
		return EToward.Left
	elseif x == -1 and y ==-1 then
		return EToward.LeftDown
	elseif x == -1 and y == 1 then
		return EToward.LeftUp
	else
		return nil
	end
end

local etowardDesCache = {}
etowardDesCache[EToward.Right] =  "正东方"
etowardDesCache[EToward.Left] = "正西方"
etowardDesCache[EToward.Up] = "正北方"
etowardDesCache[EToward.Down] = "正南方"
etowardDesCache[EToward.LeftUp] = "西北方"
etowardDesCache[EToward.LeftDown] = "西南方"
etowardDesCache[EToward.RightUp] = "东北方"
etowardDesCache[EToward.RightDown] = "东南方"

etowardDesCache = setmetatable(etowardDesCache , {__index = function (toward)
	return "未知方向"..tostring(toward)
end})

EToward.GetDes = function (toward)
	--if toward == EToward.Right then
	--	return "正东方"
	--elseif toward == EToward.RightDown then
	--	return "东南方"
	--elseif toward == EToward.RightUp then
	--	return "东北方"
	--elseif toward == EToward.Left then
	--	return "正西方"
	--elseif toward == EToward.LeftUp then
	--	return "西北方"
	--elseif toward == EToward.LeftDown then
	--	return "西南方"
	--elseif toward == EToward.Down then
	--	return "正南方"
	--elseif toward == EToward.Up then
	--	return "正北方"
	--else
	--	return "未知方向"..tostring(toward)
	--end
	return etowardDesCache[toward]
end

return EToward