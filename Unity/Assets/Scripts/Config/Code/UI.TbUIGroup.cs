
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using System.Linq;


namespace MiniDataTable.UI
{
    /// <summary>
    /// UI分组表
    /// </summary>
    public partial class TbUIGroup
    {
        private readonly System.Func<string, int, int, ByteBuf> _byteBufLoader;
        private readonly string _fileName;
        private Tables _tables;

        private readonly System.Collections.Generic.Dictionary<UI.EnumUIGroup, UI.UIGroupRow> _dataMap;
        private readonly System.Collections.Generic.Dictionary<UI.EnumUIGroup, int> _offsetMap;
        private readonly System.Collections.Generic.Dictionary<UI.EnumUIGroup, int> _lengthMap;

        private readonly System.Collections.Generic.List<UI.UIGroupRow> _dataList;
    
        public TbUIGroup(ByteBuf _buf,string fileName, System.Func<string, int, int, ByteBuf> byteBufLoader)
        {
            _dataMap = new System.Collections.Generic.Dictionary<UI.EnumUIGroup, UI.UIGroupRow>();
            _offsetMap = new System.Collections.Generic.Dictionary<UI.EnumUIGroup, int>();
            _lengthMap = new System.Collections.Generic.Dictionary<UI.EnumUIGroup, int>();
            _fileName = fileName;
            _byteBufLoader = byteBufLoader;
        
            for (int n = _buf.ReadSize(); n > 0; --n)
            {
                UI.EnumUIGroup key;
                key = (UI.EnumUIGroup)_buf.ReadInt();
                int offset = _buf.ReadInt();
                int length = _buf.ReadInt();
                _offsetMap.Add(key, offset);
                _lengthMap.Add(key, length);
            }
        }

        public void LoadAll(System.Action<UI.EnumUIGroup,UI.UIGroupRow> onLoad = null)
        {
            foreach(var key in _offsetMap.Keys)
		    {
                var value = this.Get(key);
                if (value != null)
			    {
				    onLoad?.Invoke(key, value);
			    }
		    }
        }

        public UI.UIGroupRow GetOrDefault(UI.EnumUIGroup key) => this.Get(key) ?? default;
        public UI.UIGroupRow Get(UI.EnumUIGroup key)
        {
            if (_dataMap.TryGetValue(key, out var v))
            {
                return v;
            }
            int offset = _offsetMap[key];
            int length = _lengthMap[key];
            ByteBuf buf = this._byteBufLoader(this._fileName, offset, length);
            v = UI.UIGroupRow.DeserializeUIGroupRow(buf);;
            _dataMap[key] = v;
            v.ResolveRef(_tables);
            return v;
        }
        public UI.UIGroupRow this[UI.EnumUIGroup key] => this.Get(key);

        public void ResolveRef(Tables tables)
        {
            this._tables = tables;
        }

    }

}

