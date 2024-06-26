
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
    public sealed partial class RecruitLvUpRow : Luban.BeanBase
    {
        public RecruitLvUpRow(ByteBuf _buf) 
        {
            Id = _buf.ReadInt();
            LvUpMoney = _buf.ReadInt();
            RefreshMoney = _buf.ReadInt();
            Probability = _buf.ReadFloat();
        }

        public static RecruitLvUpRow DeserializeRecruitLvUpRow(ByteBuf _buf)
        {
            return new Cultivate.RecruitLvUpRow(_buf);
        }

        /// <summary>
        /// 商店等级
        /// </summary>
        public readonly int Id;
        /// <summary>
        /// 升级需要钱数
        /// </summary>
        public readonly int LvUpMoney;
        /// <summary>
        /// 刷新需要钱数
        /// </summary>
        public readonly int RefreshMoney;
        /// <summary>
        /// 刷新老兵概率
        /// </summary>
        public readonly float Probability;
   
        public const int __ID__ = 293140538;
        public override int GetTypeId() => __ID__;

        public  void ResolveRef(Tables tables)
        {
            
            
            
            
        }

        public override string ToString()
        {
            return "{ "
            + "Id:" + Id + ","
            + "LvUpMoney:" + LvUpMoney + ","
            + "RefreshMoney:" + RefreshMoney + ","
            + "Probability:" + Probability + ","
            + "}";
        }
    }

}
