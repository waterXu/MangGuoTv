using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DouBanFMBase
{
    public class VirtualizingCollection<T> : IList<T>, IList, INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public VirtualizingCollection(int count)
        {
            this.Count = count;
        }
        private int count;
        public int Count
        {
            get
            {
                return count ;
            }
            set
            {
                count = value;
                RiasePropertyChanged("Count");
            }
        }
        private readonly Dictionary<int, IList<T>> _pages = new Dictionary<int, IList<T>>();
        public int PageSize
        {
            get { return 9; }
        }
        public T this[int index]
        {
            get
            {
                //请求的item的索引（index）所在的页码
                int pageIndex = index / PageSize;
                //滑动列表当前页产生的偏移
                int pageOffset = index % PageSize;
                RequestPage(pageIndex);
                //大于一屏的一半加载下一页
                if(pageOffset > PageSize/2 && pageIndex < Count/PageSize)
                {
                    RequestPage(pageIndex+1);
                }
                //小于一屏的一半加载上一页
                if (pageOffset < PageSize / 2 && pageIndex > 0)
                {
                    RequestPage(pageIndex -1);
                }
                //只保留当前页和上下两页，删除多余的
                if (_pages.Count >= 3 && _pages.ContainsKey(pageIndex - 2))
                {
                    _pages.Remove(pageIndex-2);
                }
                if (_pages.Count > 3 && _pages.ContainsKey(pageIndex + 2))
                {
                    _pages.Remove(pageIndex+2);
                }
                Debug.WriteLine("当前页码：" + pageIndex.ToString());
                Debug.WriteLine("当前内存页数：" + _pages.Count);
                return _pages[pageIndex][pageOffset];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        protected virtual void RequestPage(int pageIndex)
        {
            PopulatePage(pageIndex, FetchPage(pageIndex));
        }

        protected virtual void PopulatePage(int pageIndex, IList<T> page)
        {
            if (!_pages.ContainsKey(pageIndex))
            {
                _pages[pageIndex] = page;
            }
        }
             
        private IList<T> FetchPage(int pageIndex)
        {
            //return (IList<T>)LocalData.FetchRange(pageIndex*PageSize,PageSize);
            return (IList<T>)null;
        }
      
        public event PropertyChangedEventHandler PropertyChanged;
        private void RiasePropertyChanged(string p)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(p));
            }
        }
        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }



        public void Add(T item)
        {

            var newPos = Count;
            Count++;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var handler = CollectionChanged;
                if (handler == null) return;
                handler(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, newPos));
            });
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }



        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }



        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }
        //以下实现IList
        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }
    }
}
