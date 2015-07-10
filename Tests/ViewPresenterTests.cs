using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using MbUnit.Framework;
using Moq;

using DotNetNuke.Modules.DNNUnitOfWork.Components;
using DotNetNuke.Modules.DNNUnitOfWork.Models;
using DotNetNuke.Modules.DNNUnitOfWork.Presenters;
using DotNetNuke.Modules.DNNUnitOfWork.Views;

namespace DotNetNuke.Modules.DNNUnitOfWork.Tests
{
    [TestFixture]
    public class ViewPresenterTests
    {
        private Mock<IItemRepository> mockStore;

        [Test]
        public void Delete_DeleteItemsFromDifferentModules_ItemBelongingToModuleIsDeleted()
        {
            //arrange
            int moduleId = 1;
            mockStore = _MockStores.ItemRepositoryFake();
            mockStore.Object.CreateItem(new Item { ItemId = 1, ItemName = "Item 1", ItemDescription = "Item 1 Description", AssignedUserId = 0, ModuleId = 1, CreatedByUserId = 0, CreatedOnDate = DateTime.Now.AddDays(-1), LastModifiedByUserId = 0, LastModifiedOnDate = DateTime.Now });
            mockStore.Object.CreateItem(new Item { ItemId = 2, ItemName = "Item 2", ItemDescription = "Item 2 Description", AssignedUserId = 0, ModuleId = 1, CreatedByUserId = 0, CreatedOnDate = DateTime.Now.AddDays(-1), LastModifiedByUserId = 0, LastModifiedOnDate = DateTime.Now });
            mockStore.Object.CreateItem(new Item { ItemId = 3, ItemName = "Item 3", ItemDescription = "Item 3 Description", AssignedUserId = 0, ModuleId = 2, CreatedByUserId = 0, CreatedOnDate = DateTime.Now.AddDays(-1), LastModifiedByUserId = 0, LastModifiedOnDate = DateTime.Now });
            mockStore.Object.CreateItem(new Item { ItemId = 4, ItemName = "Item 4", ItemDescription = "Item 4 Description", AssignedUserId = 0, ModuleId = 2, CreatedByUserId = 0, CreatedOnDate = DateTime.Now.AddDays(-1), LastModifiedByUserId = 0, LastModifiedOnDate = DateTime.Now });

            var mockView = ItemListViewFake(moduleId);
            var presenter = new ViewPresenter(mockView.Object);

            //act
            presenter.ItemRepository = mockStore.Object;
            presenter.SettingsRepository = _MockStores.ModuleSettingsFake().Object;
            presenter.Delete(null, new DeleteClickEventArgs { ItemId = 3, ModuleId = moduleId });
            presenter.Delete(null, new DeleteClickEventArgs { ItemId = 1, ModuleId = moduleId });

            //assert
            Assert.IsTrue(mockStore.Object.GetItem(1, moduleId) == null); //Item 1 is deleted because it belongs to module 1
            Assert.IsTrue(mockStore.Object.GetItem(3, moduleId) != null); //Item 3 is not deleted because it does not belong to module 1
        }

        public Mock<IItemListView> ItemListViewFake(int moduleId)
        {
            var mock = new Mock<IItemListView>();
            mock.Setup(x => x.IsEditable).Returns(true);
            mock.Setup(x => x.ItemList).Returns(mockStore.Object.GetItems(moduleId));
            return mock;
        }

    }
}