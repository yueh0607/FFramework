
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;


namespace MiniDataTable.Battle
{
    public sealed partial class PropertiesRow : Luban.BeanBase
    {
        public PropertiesRow(ByteBuf _buf) 
        {
            Id = _buf.ReadInt();
            Atk = _buf.ReadInt();
            Def = _buf.ReadInt();
            Hp = _buf.ReadInt();
            Mp = _buf.ReadInt();
            Agility = _buf.ReadInt();
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

        public static PropertiesRow DeserializePropertiesRow(ByteBuf _buf)
        {
            return new Battle.PropertiesRow(_buf);
        }

        /// <summary>
        /// 职业编号
        /// </summary>
        public readonly int Id;
        /// <summary>
        /// 战斗力
        /// </summary>
        public readonly int Atk;
        /// <summary>
        /// 防御力
        /// </summary>
        public readonly int Def;
        /// <summary>
        /// 生命值
        /// </summary>
        public readonly int Hp;
        /// <summary>
        /// 法力值
        /// </summary>
        public readonly int Mp;
        /// <summary>
        /// 敏捷
        /// </summary>
        public readonly int Agility;
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
   
        public const int __ID__ = 1328746737;
        public override int GetTypeId() => __ID__;

        public  void ResolveRef(Tables tables)
        {
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
        }

        public override string ToString()
        {
            return "{ "
            + "Id:" + Id + ","
            + "Atk:" + Atk + ","
            + "Def:" + Def + ","
            + "Hp:" + Hp + ","
            + "Mp:" + Mp + ","
            + "Agility:" + Agility + ","
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