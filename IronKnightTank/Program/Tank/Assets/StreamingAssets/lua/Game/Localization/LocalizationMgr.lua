---------------------------------------------
--- LocalizatinMgr
--- Created by thrt520.
--- DateTime: 2018/6/25
---------------------------------------------
---@class LocalizatinMgr
local LocalizatinMgr = {}

ELanguageType = require("Game.Localization.Enum.ELanguageType")

local curLang = ELanguageType.Cn
local curLangKey
local LangKeyDic = {}

function LocalizatinMgr.Init()
	LangKeyDic[ELanguageType.Cn] = DataConst.Language_Field.Text_Ch
	LangKeyDic[ELanguageType.En] = DataConst.Language_Field.Text_En
	curLangKey = LangKeyDic[curLang]
end


function LocalizatinMgr.GetDes(id)
	if not id then
		clog("id nil wrong")
	end
	local tData = JsonDataMgr.GetLanguageData(id)
	if not tData then
		clog("tData nil "..tostring(id))
	else
		return tData[curLangKey]
	end
end

function LocalizatinMgr.ChangeCurLang(lang)
	curLang = lang
	curLangKey = LangKeyDic[lang]
end

return LocalizatinMgr