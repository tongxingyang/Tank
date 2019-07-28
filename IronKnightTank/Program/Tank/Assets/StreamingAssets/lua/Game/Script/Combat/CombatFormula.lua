local BattleScript = {}
---------------攻击顺序判定函数----------------------
--[[function BattleScript.Attack_Order(ShootingSpeed , LoadingSpeed , RotationPenalty )

    local  Attack_OrderTrue = ShootingSpeed + LoadingSpeed - RotationPenalty
    return Attack_OrderTrue 
end--]]

-------------攻击命中判断函数------------------------------------------
function BattleScript.Attack_Hit(ProjectionAmount, TargetDistance , TerrainCovert , UnitHit , hitPrefer , AimedShot )
    --TargetDistance = 4
    --clog("ProjectionAmount, TargetDistance , TerrainCovert , UnitHit"..tostring(ProjectionAmount).."   "..tostring(TargetDistance).."  "..
    --        tostring(TerrainCovert).."   "..tostring(UnitHit))
    local hitRes --- 攻击结果
    local randomNum  --- 受击方随机值
    local projectTrue --- 投影量修正值
    local hitTrue --- 命中修正后投影量
    local unitHitTrue --- 命中修正值
    local IsHit   ---是否命中
    projectTrue = (ProjectionAmount * (1- TargetDistance *(TargetDistance - 1)*0.02/2))*TerrainCovert/100
    unitHitTrue = UnitHit * (1-0.05*(TargetDistance - 1 ))
    hitTrue = (projectTrue + unitHitTrue) / 200
    hitTrue = hitTrue * (AimedShot and 1.2 or 1)

    if hitTrue >= 1 then
        IsHit = true
    else
        randomNum = math.random()
        if randomNum < hitTrue then
            IsHit = true
        else
            IsHit = false
        end

    end

    if IsHit then
        local randomHitRes = math.random(100)
        if randomHitRes  <= 50 + hitPrefer  then
            hitRes =  1 ---命中炮塔
        else
            hitRes =  2 ----命中车体
        end
    else
        hitRes = 0
    end
    return hitRes , randomNum , projectTrue , hitTrue , unitHitTrue
end
------------------------------幸运判定函数---------------------------
function BattleScript.Luckly(LuckNum)
    local Num5 =  math.random(1,100)
    if LuckNum >  Num5 then
        return 1 --- 发生了跳弹
    end
    return 0 ---没有发生跳弹进入伤害判定
end

-------------AI攻击命中判断函数------------------------------------------
--[[function BattleScript.AI_Attack_Hit(ProjectionAmount, TargetDistance , TerrainCovert , UnitHit )

    local projectTrue --- 投影量修正值
    local AI_hitTrue --- 命中修正后投影量
    local unitHitTrue --- 命中修正值
    projectTrue = ProjectionAmount * (1 - 0.05 * ( TargetDistance - 1))*TerrainCovert / 100
    unitHitTrue = UnitHit * (1 - 0.05 * ( TargetDistance - 1) )
    AI_hitTrue = (projectTrue + unitHitTrue) / 200 * (1-0.1*(TargetDistance - 1))

    return   AI_hitTrue , projectTrue , unitHitTrue
end--]]

------------------------------伤害判定函数---------------------------
---TankDamage 火炮伤害 TankArmor 坦克护甲

local dmgFloatWide = 0.2

function BattleScript.Attack_Result(TankDamage, TankArmor,Weakness)
    --local num2 = math.random(-20,20)
    --local TankDamageTrue = TankDamage * (100 - num2)/100 ----对坦克的伤害进行进一步的浮动修正
    --local WeaknessNum = Weakness and 1 or 0
    --local TankArmor =
    --TankArmor = TankArmor*(1-(WeaknessNum * 0.9))
    local r = 1 + math.random() * 2 * dmgFloatWide - dmgFloatWide
    local dmgTrue = TankDamage * r
    local dmgRes
    local DamageTrue = dmgTrue - TankArmor
    if DamageTrue > 0 then
        dmgRes = 1 ------击毁
    else
      local num1 = math.random(-1,-10)
        if DamageTrue > num1 then
            dmgRes =  2 -----------造成损坏
        else
            dmgRes =  3 ------------一点事都没有
        end
    end
    return dmgRes , TankArmor
end

function BattleScript.DestoryRate(tankDamage , tankArmor)
    --clog("tankDamage"..tostring(tankDamage).."  tankArmor"..tostring(tankArmor))
    local targetTankDamage = tankArmor
    local rate = targetTankDamage / tankDamage
    local destoryRate = (1 + dmgFloatWide - rate ) / (dmgFloatWide * 2 )
    destoryRate = math.clamp(destoryRate , 0 , 1)
    return destoryRate
end

function BattleScript.InitiativeSkillDamage(InitiativeSkillHit,InitiativeSkillDamageType)
    local InitiativeSkill_num = math.random(1,100)
    local SkillDagRes
    if InitiativeSkillDamageType == 1 then ------高爆弹
        if InitiativeSkillHit > InitiativeSkill_num  then
            local Num2 = math.random(1,100)
            if Num2 <= 50 then
                SkillDagRes = 2 ---车体损坏
            else
                SkillDagRes = 3 -----炮塔损坏
            end
        else
            SkillDagRes = 1 -----未命中
        end

    end

    if InitiativeSkillDamageType == 2 then  -----穿甲弹和轰炸
        if InitiativeSkillHit > InitiativeSkill_num  then
            SkillDagRes = 4 --------摧毁
        else
            SkillDagRes = 1 -----未命中
        end
        
    end
    return SkillDagRes
end

return BattleScript