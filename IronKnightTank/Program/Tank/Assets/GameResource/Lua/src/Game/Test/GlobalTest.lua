---------------------------------------------
--- GlobalTest
--- Created by thrt520.
--- DateTime: 2018/6/25
---------------------------------------------

function treaverse_global_env( t , level )
	for i, v in pairs(t) do
		local preFix = string.rep(" " , level * 5)
		print(string.format("%s%s(%s)" , preFix , i , type(v)))
		if(type(v) == "table") and i ~= "_G"   then
			--treaverse_global_env(v , level + 1)
		elseif (type(v) == "table") then
			--print(string.format("%sSKIPTABLE:%S" , preFix , i))
		end
	end

end

treaverse_global_env(_G , 0)