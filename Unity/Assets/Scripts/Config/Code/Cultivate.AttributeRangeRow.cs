
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;


namespace MiniDataTable.Cultivate
{
    public sealed partial class AttributeRangeRow : Luban.BeanBase
    {
        public AttributeRangeRow(ByteBuf _buf) 
        {
            Id = _buf.ReadInt();
            AtkMin = _buf.ReadInt();
            AtkMax = _buf.ReadInt();
            DefMin = _buf.ReadInt();
            DefMax = _buf.ReadInt();
            HpMin = _buf.ReadInt();
            HpMax = _buf.ReadInt();
            MpMin = _buf.ReadInt();
            MpMax = _buf.ReadInt();
            AgilityMin = _buf.ReadInt();
            AgilityMax = _buf.ReadInt();
            AttackRange = _buf.ReadInt();
            Strength = _buf.ReadInt();
            Weight = _buf.ReadInt();
            Level = _buf.ReadInt();
            Exp = _buf.ReadInt();
            PassiveEffect = _buf.ReadInt();
            EvasionRate = _buf.ReadInt();
            BlockRate = _buf.ReadInt();
            ReboundRate = _buf.ReadInt();
        }

        public static AttributeRangeRow DeserializeAttributeRangeRow(ByteBuf _buf)
        {
            return new Cultivate.AttributeRangeRow(_buf);
        }

        /// <summary>
        /// 商店等级
        /// </summary>
        public readonly int Id;
        /// <summary>
        /// 战斗力下限
        /// </summary>
        public readonly int AtkMin;
        /// <summary>
        /// 战斗力上限
        /// </summary>
        public readonly int AtkMax;
        /// <summary>
        /// 防御力下限
        /// </summary>
        public readonly int DefMin;
        /// <summary>
        /// 防御力上限
        /// </summary>
        public readonly int DefMax;
        /// <summary>
        /// 生命值下限
        /// </summary>
        public readonly int HpMin;
        /// <summary>
        /// 生命值上限
        /// </summary>
        public readonly int HpMax;
        /// <summary>
        /// 法力值下限
        /// </summary>
        public readonly int MpMin;
        /// <summary>
        /// 法力值上限
        /// </summary>
        public readonly int MpMax;
        /// <summary>
        /// 敏捷下限
        /// </summary>
        public readonly int AgilityMin;
        /// <summary>
        /// 敏捷上限
        /// </summary>
        public readonly int AgilityMax;
        /// <summary>
        /// 攻击距离
        /// </summary>
        public readonly int AttackRange;
        /// <summary>
        /// 力量
        /// </summary>
        public readonly int Strength;
        /// <summary>
        /// 负重
        /// </summary>
        public readonly int Weight;
        /// <summary>
        /// 等级
        /// </summary>
        public readonly int Level;
        /// <summary>
        /// 经验
        /// </summary>
        public readonly int Exp;
        /// <summary>
        /// 被动效果
        /// </summary>
        public readonly int PassiveEffect;
        /// <summary>
        /// 闪避率
        /// </summary>
        public readonly int EvasionRate;
        /// <summary>
        /// 格挡率
        /// </summary>
        public readonly int BlockRate;
        /// <summary>
        /// 招架率
        /// </summary>
        public readonly int ReboundRate;
   
        public const int __ID__ = -2110329930;
        public override int GetTypeId() => __ID__;

        public  void ResolveRef(Tables tables)
        {
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
        }

        public override string ToString()
        {
            return "{ "
            + "Id:" + Id + ","
            + "AtkMin:" + AtkMin + ","
            + "AtkMax:" + AtkMax + ","
            + "DefMin:" + DefMin + ","
            + "DefMax:" + DefMax + ","
            + "HpMin:" + HpMin + ","
            + "HpMax:" + HpMax + ","
            + "MpMin:" + MpMin + ","
            + "MpMax:" + MpMax + ","
            + "AgilityMin:" + AgilityMin + ","
            + "AgilityMax:" + AgilityMax + ","
            + "AttackRange:" + AttackRange + ","
            + "Strength:" + Strength + ","
            + "Weight:" + Weight + ","
            + "Level:" + Level + ","
            + "Exp:" + Exp + ","
            + "PassiveEffect:" + PassiveEffect + ","
            + "EvasionRate:" + EvasionRate + ","
            + "BlockRate:" + BlockRate + ","
            + "ReboundRate:" + ReboundRate + ","
            + "}";
        }
    }

}