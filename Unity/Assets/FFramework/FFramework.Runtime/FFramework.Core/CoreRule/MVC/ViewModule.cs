namespace FFramework
{
    public struct EmptyStruct
    {

    }
    public abstract class ViewModuleBase 
    {

        public async FTask<T> LoadView<T>(string viewPath) where T : class, IView
        {
            return await LoadView<T, EmptyStruct>(viewPath);
        }

        public async FTask<T> LoadView<T, K>(string viewPath, K parameters = default) where T : class, IView where K : struct
        {
            T viewAsset = await OnLoadViewAsset<T>(viewPath);
            viewAsset.Send<IViewLoad<K>, K>(parameters);
            return viewAsset;
        }

        public void UnloadView<T>(IView view) where T : class, IView
        {
            UnloadView<T, EmptyStruct>(view);
        }

        public void UnloadView<T, K>(IView view, K parameters = default) where T : class, IView where K : struct
        {
            view.Send<IViewUnload<K>, K>(parameters);
            OnUnloadViewAsset(view);
        }


        /// <summary>
        /// 用于加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewPath"></param>
        /// <returns></returns>
        protected abstract FTask<T> OnLoadViewAsset<T>(string viewPath) where T : class, IView;

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="view"></param>
        protected abstract void OnUnloadViewAsset(IView view);
    }
}
