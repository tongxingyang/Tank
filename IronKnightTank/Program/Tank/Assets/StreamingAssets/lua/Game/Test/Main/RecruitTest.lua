---------------------------------------------
--- RecruitTest
--- Created by thrt520.
--- DateTime: 2018/10/15
---------------------------------------------
local RecruitTest = {}

RecruitTest.Test = function()
	local dataList = DataMgr.GetRecruitYounSoliderList(10)
	for _, data in pairs(dataList) do
		clog("======================================")
		for i, v in pairs(data) do
			clog("i: "..tostring(i).."   v:"..tostring(v))
		end
	end
end

return RecruitTest