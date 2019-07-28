---------------------------------------------
--- MathTools
--- Created by thrt520.
--- DateTime: 2018/5/30
---------------------------------------------
math.clamp = function(val, left , right)
	if left and  val < left then
		return left
	end
	if right and val > right then
		return right
	end
	return val
end

---除法求整 省去小数点以后的
math.divsion2Int = function(left , right)
	return math.floor(left / right)
end

----正态分布随机数   期望值为0.5  范围为0~1
math.boxMullerRandom = function()
	local u = math.random()
	local v = math.random()
	local  z = math.sqrt(-2 * math.log(u)) * math.cos( 2 * math.pi * v)
	z = (z + 3) / 6
	z = math.clamp(z , 0 , 1 )
	return z
end

----返回x被y整除的余数和整除结果
math.mod = function(x , y)
	local a =  math.fmod(x , y)
	local b = (x - a) / y
	return a , b
end


---计算角度 返回数值在0~180
math.calAngle = function(a , b)
	local c = math.abs(a - b)
	c = math.fmod(c , 360)
	c = 180 - math.abs(180 - c)
	return c
end
