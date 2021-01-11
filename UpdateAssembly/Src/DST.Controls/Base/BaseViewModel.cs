// ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
// 文件名称(File Name)：      BaseViewModel.cs
// 功能描述(Description)：    所有ViewModel的父类
// 数据表(Tables)：		      无
// 作者(Author)：             DST-
// 日期(Create Date)：        2020-08-17 13:28
// R1:
//    修改作者:
//    修改日期:
//    修改理由:
//＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
using DST.Common.Logger;
using DST.Common.Model;
using DST.Database.WPFCommonModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DST.Controls.Base
{
    public abstract class BaseViewModel : ViewModelBase
    {
        /// <summary>
        /// 是否已经保存过
        /// </summary>
        private bool IsSaved = false;

        /// <summary>
        /// 在创建窗口时传递过来的默认参数
        /// </summary>
        public object[] Args { get; set; }

        /// <summary>
        /// 额外的数据
        /// </summary>
        public object ExtraData { get; set; }

        /// <summary>
        /// 窗口返回的结果数据
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// 无参异步关闭窗口的委托定义，用以在多线程情况下关闭窗口
        /// </summary>
        public Action CloseContentWindowDelegate { get; set; }

        /// <summary>
        /// 有参异步关闭窗口的委托定义，用以在多线程情况下关闭窗口
        /// </summary>
        public Action<object> CloseContentWindowDelegateEx { get; set; }

        /// <summary>
        /// 无参构造
        /// </summary>
        public BaseViewModel()
        {
        }

        /// <summary>
        /// 检查是否有更新数据
        /// </summary>
        protected virtual bool CheckDataChange()
        {
            return false;
        }

        /// <summary>
        /// 检查页面数据是否完整
        /// </summary>
        protected virtual bool CheckData()
        {
            return true;
        }

        /// <summary>
        /// 根据窗体名，载入窗体,同时设置窗体的宽和高
        /// </summary>
        public void ShowContentWindow(string windowName, string title, int width, int height)
        {
            //var message = new ShowContentWindowMessage(windowName, title);
            //message.Height = height;
            //message.Width = width;
            //Messenger.Default.Send<ShowContentWindowMessage>(message);
        }

        /// <summary>
        /// 注册键盘消息
        /// </summary>
        public void RegisterKeyBoardMessage()
        {
        }

        /// <summary>
        /// 卸载键盘消息
        /// </summary>
        public void UnRegisterKeyBoardMessage()
        {
        }

        /// <summary>
        /// 键盘响应事件，提供各个模块调用
        /// </summary>
        protected virtual void KeyBoardMessage(string keyCode)
        {
        }

        /// <summary>
        /// 键盘响应公共消息，Home，Back，Save，Delete等等
        /// </summary>
        protected virtual void PublicKeyBoardMessage(string keyCode)
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                switch (keyCode)
                {
                    case "Insert":
                        InsertData();
                        break;

                    case "Delete":
                        DeleteData();
                        break;

                    case "Save":
                        if (CheckData())//检查数据是否完整
                        {
                            //此处省略检查数据是否修改过，既然点击保存就执行保存操作
                            SaveResult saveResult = SaveData();
                            if (saveResult == SaveResult.Fail)
                            {
                                ShowMessageBox("数据保存失败。", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else if (saveResult == SaveResult.Success)
                            {
                                ShowMessageBox("数据保存成功。", MessageBoxButton.OK, MessageBoxImage.Information);
                                IsSaved = true;
                            }
                            else if (saveResult == SaveResult.CancelMessageBox)
                            {
                                IsSaved = true;
                            }
                            if (IsSaved)
                            {
                                this.CloseContentWindow();
                                //Messenger.Default.Send<object>(this, EnumMessageKey.RefreshInOperationWindow);
                            }
                        }

                        break;

                    case "HOME":
                        this.CloseContentWindow();
                        //Messenger.Default.Send<object>(this, EnumMessageKey.CloseInOperationWindow);
                        break;

                    case "Back":
                        KeyBack();
                        break;
                }
            }
            catch (Exception ex)
            {
                WhirlingControlManager.CloseWaitingForm();
                Logger.Error("程序异常", ex);
                ShowMessageBox(ex.Message, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }

        /// <summary>
        /// 载入数据
        /// </summary>
        public virtual void LoadData()
        {
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        protected virtual SaveResult SaveData()
        {
            return SaveResult.Success;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        protected virtual bool DeleteData()
        {
            return true;
        }

        /// <summary>
        /// 新增事件
        /// </summary>
        protected virtual bool InsertData()
        {
            return true;
        }

        /// <summary>
        /// 返回按键
        /// </summary>
        protected virtual void KeyBack()
        {
            this.CloseContentWindow();
        }

        /// <summary>
        /// 载入窗口，用作接受参数
        /// </summary>
        public virtual void OnViewLoaded()
        {
        }

        /// <summary>
        /// 卸载窗口
        /// </summary>
        public virtual void OnPreviewViewUnLoaded(CancelEventArgs e)
        {
            // 已经保存过了
            if (IsSaved)
            {
                return;
            }

            try
            {
                {
                    if (CheckDataChange())//检查数据是否修改过
                    {
                        if (CheckData())//检查数据是否完整
                        {
                            ShowMessageBox("当前数据有修改，是否保存数据？",
                                           MessageBoxButton.YesNoCancel,
                                           MessageBoxImage.Question,
                                           new Action<MessageBoxResult>((r) =>
                                           {
                                               if (r == MessageBoxResult.Yes || r == MessageBoxResult.OK)
                                               {
                                                   SaveResult saveResult = SaveResult.Success;
                                                   try
                                                   {
                                                       WhirlingControlManager.ShowWaitingForm();
                                                       saveResult = SaveData();
                                                   }
                                                   finally
                                                   {
                                                       WhirlingControlManager.CloseWaitingForm();
                                                   }
                                                   if (saveResult == SaveResult.Fail)
                                                   {
                                                       ShowMessageBox("数据保存失败。", MessageBoxButton.OK, MessageBoxImage.Error);
                                                   }
                                               }
                                               else if (r == MessageBoxResult.No)
                                               {
                                               }
                                               else
                                               {
                                                   e.Cancel = true;
                                               }
                                           }));
                        }
                        else
                        {
                            // 检查数据不完整，并且有修改 不关闭窗口
                            e.Cancel = true;
                        }
                    }
                }

                // 关闭窗口时，清除当前打开窗体对象
                if (!e.Cancel)
                {
                    this.ResetCurntOpenForm();
                    Messenger.Default.Unregister<string>(this);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("程序异常", ex);
                ShowMessageBox("系统异常，中断该操作", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 重置CurntOpenForm，但有的界面在关闭时无需重置，需要重写该方法
        /// </summary>
        public virtual void ResetCurntOpenForm()
        {
        }

        /// <summary>
        /// 卸载窗口，注销键盘按键事件，防止多次注册导致多次调用
        /// </summary>
        public virtual void OnViewUnLoaded()
        {
            Messenger.Default.Unregister<dynamic>(this);
            UnRegisterKeyBoardMessage();
        }

        /// <summary>
        /// 虚方法：键盘按下事件
        /// </summary>
        public virtual void KeyDown(object sender, KeyEventArgs e)
        {
        }

        /// <summary>
        /// 虚方法：窗口丢失焦点事件
        /// </summary>
        public virtual void Control_LostFocus(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 关闭窗口，使用异步委托
        /// </summary>
        protected virtual void CloseContentWindow()
        {
            if (CloseContentWindowDelegate != null)
            {
                CloseContentWindowDelegate();
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        protected void SendMessage<T>(T message)
        {
            MessengerInstance.Send<T>(message);
        }

        /// <summary>
        /// 向View层发送消息弹出MessageBox
        /// </summary>
        public void ShowMessageBox(string subMessage = "",
                                   MessageBoxButton button = MessageBoxButton.OK,
                                   MessageBoxImage icon = MessageBoxImage.None,
                                   Action<MessageBoxResult> callBack = null,
                                   bool isAutoClose = false,
                                   int autoCloseTime = 2000,
                                   bool isAsyncShow = false)
        {
            SendMessage<ShowMessageBoxMessage>(new ShowMessageBoxMessage("预留Title", isAutoClose, subMessage, button, icon, callBack, autoCloseTime, isAsyncShow));
        }

        /// <summary>
        /// 载入字典数据
        /// </summary>
        protected virtual void LoadDictData()
        {
        }

        /// <summary>
        /// 人员名称转换
        /// </summary>
        protected virtual string ShowUserName(string code)
        {
            string value = code;

            return value;
        }

        /// <summary>
        /// 获取动态对象
        /// </summary>
        public dynamic GetDynamicObject(Dictionary<string, object> properties)
        {
            return new CustomDynamicObject(properties);
        }

        #region

        /// <summary>
        /// 自定义动态对象
        /// </summary>
        public class CustomDynamicObject : DynamicObject, INotifyPropertyChanged, ICustomTypeDescriptor
        {
            #region DynamicObject
            public readonly Dictionary<string, object> _properties;                        // 字典信息

            public event PropertyChangedEventHandler PropertyChanged;                      // 属性值更改事件

            /// <summary>
            /// 带参的构造方法
            /// </summary>
            /// <param name="properties">字典</param>
            public CustomDynamicObject(Dictionary<string, object> properties)
            {
                _properties = properties;
            }

            /// <summary>
            /// 根据字典的KEY获取对应的Value
            /// </summary>
            public object GetMemberValue(string name)
            {
                if (_properties.ContainsKey(name))
                {
                    return _properties[name];
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// 字典新增数据
            /// </summary>
            public bool AddProp(string key, object value)
            {
                if (!_properties.ContainsKey(key))
                {
                    _properties.Add(key, value);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// 重写方法：获取字典的所有KEY
            /// </summary>
            public override IEnumerable<string> GetDynamicMemberNames()
            {
                return _properties.Keys;
            }

            /// <summary>
            /// 重写方法：根据属性，获取字典中的值
            /// </summary>
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if (_properties.ContainsKey(binder.Name))
                {
                    result = _properties[binder.Name];
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            /// <summary>
            /// 重写方法：根据属性信息和新值，重新设置字典中对应KEY的值
            /// </summary>
            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                if (_properties.ContainsKey(binder.Name))
                {
                    if (!_properties[binder.Name].Equals(value))
                    {
                        _properties[binder.Name] = value;
                        OnPropertyChanged(binder.Name);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// 方法：实现接口INotifyPropertyChanged
            /// </summary>
            protected void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            /// <summary>
            /// 直接根据KEY获取值
            /// </summary>
            public object this[string name]
            {
                get
                {
                    if (_properties.ContainsKey(name))
                    {
                        return _properties[name];
                    }
                    return null;
                }
                set
                {
                    _properties[name] = value;
                }
            }

            /// <summary>
            /// 获取属性
            /// </summary>
            public AttributeCollection GetAttributes()
            {
                return AttributeCollection.Empty;
            }

            /// <summary>
            /// 获取类型名称
            /// </summary>
            public string GetClassName()
            {
                return null;
            }

            /// <summary>
            /// 获取空间信息
            /// </summary>
            public string GetComponentName()
            {
                return null;
            }

            public TypeConverter GetConverter()
            {
                return null;
            }

            public EventDescriptor GetDefaultEvent()
            {
                return null;
            }

            public PropertyDescriptor GetDefaultProperty()
            {
                return null;
            }

            public object GetEditor(Type editorBaseType)
            {
                return null;
            }

            public EventDescriptorCollection GetEvents(Attribute[] attributes)
            {
                return EventDescriptorCollection.Empty;
            }

            public EventDescriptorCollection GetEvents()
            {
                return EventDescriptorCollection.Empty;
            }

            public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
            {
                return new PropertyDescriptorCollection(_properties.Keys.Select(key => new MyDescriptor(key)).ToArray());
            }

            public PropertyDescriptorCollection GetProperties()
            {
                return GetProperties(null);
            }

            public object GetPropertyOwner(PropertyDescriptor pd)
            {
                return this;
            }

            #endregion DynamicObject
        }

        /// <summary>
        /// 自定义属性描述类
        /// </summary>
        public class MyDescriptor : PropertyDescriptor
        {
            public MyDescriptor(string name) : base(name, null)
            {
            }

            public override bool CanResetValue(object component)
            {
                return true;
            }

            public override Type ComponentType
            {
                get { return typeof(CustomDynamicObject); }
            }

            public override object GetValue(object component)
            {
                return (component as CustomDynamicObject)[Name];
            }

            public override bool IsReadOnly
            {
                get { return false; }
            }

            public override Type PropertyType
            {
                get { return typeof(object); }
            }

            public override void ResetValue(object component)
            {
                (component as CustomDynamicObject)._properties.Remove(Name);
            }

            public override void SetValue(object component, object value)
            {
                (component as CustomDynamicObject)[Name] = value;
            }

            public override bool ShouldSerializeValue(object component)
            {
                return (component as CustomDynamicObject)._properties.ContainsKey(Name);
            }
        }

        #endregion
    }
}