//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class UpdateManager : SingletonTemporary<UpdateManager>
//{
//    private HashSet<IUpdateManager> _table = new HashSet<IUpdateManager>();
//    private List<IUpdateManager> _toRemove = new List<IUpdateManager>();

//    public void Register(IUpdateManager obj)
//    {
//        _table.Add(obj);
//    }
//    public void UnRegister(IUpdateManager obj)
//    {
//        _toRemove.Add(obj);
//    }

//    private void Update()
//    {
//        foreach (var item in _toRemove)
//        {
//            _table.Remove(item);
//        }
//        _toRemove.Clear();

//        foreach (IUpdateManager obj in _table)
//        {
//            obj.UpdateMe(); 
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : SingletonTemporary<UpdateManager>
{
    // Dùng List để lặp nhanh hơn và tránh sinh rác (GC)
    private readonly List<IUpdateManager> _activeList = new List<IUpdateManager>();

    // Danh sách tạm để hứng các đối tượng đăng ký/hủy trong frame
    private readonly List<IUpdateManager> _toAdd = new List<IUpdateManager>();
    private readonly List<IUpdateManager> _toRemove = new List<IUpdateManager>();

    private bool _isUpdating = false; // Đánh dấu xem có đang trong vòng lặp Update không

    public void Register(IUpdateManager obj)
    {
        if (_isUpdating)
        {
            _toAdd.Add(obj);
        }
        else if (!_activeList.Contains(obj))
        {
            _activeList.Add(obj);
        }
    }

    public void UnRegister(IUpdateManager obj)
    {
        if (_isUpdating)
        {
            // Tránh việc add trùng vào list xóa
            if (!_toRemove.Contains(obj)) _toRemove.Add(obj);
        }
        else
        {
            _activeList.Remove(obj);
        }
    }

    private void Update()
    {
        _isUpdating = true;

        // Lặp bằng for-index thông thường: Nhanh nhất trong C#, không sinh rác (GC Alloc)
        for (int i = 0; i < _activeList.Count; i++)
        {
            // Kiểm tra null phòng trường hợp Object bị Destroy bất ngờ không qua OnDisable
            if (_activeList[i] != null)
            {
                _activeList[i].UpdateMe();
            }
        }

        _isUpdating = false;

        // Xử lý dọn dẹp NGAY CUỐI FRAME để frame sau sạch sẽ
        PostUpdateCleanUp();
    }

    private void PostUpdateCleanUp()
    {
        // 1. Xóa các phần tử cần xóa
        if (_toRemove.Count > 0)
        {
            for (int i = 0; i < _toRemove.Count; i++)
            {
                _activeList.Remove(_toRemove[i]);
                _toAdd.Remove(_toRemove[i]); // Phòng trường hợp vừa Add vừa Remove trong cùng 1 frame
            }
            _toRemove.Clear();
        }

        // 2. Thêm các phần tử mới vào
        if (_toAdd.Count > 0)
        {
            for (int i = 0; i < _toAdd.Count; i++)
            {
                if (!_activeList.Contains(_toAdd[i]))
                {
                    _activeList.Add(_toAdd[i]);
                }
            }
            _toAdd.Clear();
        }
    }
}
