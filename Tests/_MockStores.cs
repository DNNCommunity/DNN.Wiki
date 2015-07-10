using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetNuke.Modules.DNNUnitOfWork.Tests
{
    public class _MockStores
    {
        static public Mock<IItemRepository> ItemRepositoryFake()
        {
            var _allItems = new List<Item>();
            var mock = new Mock<IItemRepository>();

            mock.Setup(x => x.CreateItem(It.IsAny<Item>()))
                .Callback((Item i) =>
                {
                    _allItems.Add(i);
                });
            mock.Setup(x => x.DeleteItem(It.IsAny<int>(), It.IsAny<int>()))
                .Callback((int id, int mid) =>
                {
                    var remItem = _allItems.Where(i => i.ItemId == id).FirstOrDefault();
                    _allItems.Remove(remItem);
                });
            mock.Setup(x => x.GetItems(It.IsAny<int>()))
                .Returns((int mid) => _allItems.Where(x => x.ModuleId == mid));
            mock.Setup(x => x.GetItem(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int id, int mid) => _allItems.Where(i => i.ItemId == id).FirstOrDefault());
            return mock;
        }

        static public Mock<ISettingsRepository> ModuleSettingsFake()
        {
            var _allItems = new List<Item>();
            var mock = new Mock<ISettingsRepository>();

            mock.Setup(x => x.MaxItems).Returns(5);
            return mock;
        }
    }
}