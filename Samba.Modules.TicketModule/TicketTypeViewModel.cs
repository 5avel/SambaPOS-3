﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using FluentValidation;
using Samba.Domain.Models.Accounts;
using Samba.Domain.Models.Menus;
using Samba.Domain.Models.Settings;
using Samba.Domain.Models.Tickets;
using Samba.Localization.Properties;
using Samba.Presentation.Common.ModelBase;
using Samba.Presentation.Services;
using Samba.Services;

namespace Samba.Modules.TicketModule
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    class TicketTypeViewModel : EntityViewModelBase<TicketType>
    {
        private readonly IMenuService _menuService;

        [ImportingConstructor]
        public TicketTypeViewModel(IMenuService menuService)
        {
            _menuService = menuService;
        }

        private IEnumerable<ScreenMenu> _screenMenus;
        public IEnumerable<ScreenMenu> ScreenMenus
        {
            get { return _screenMenus ?? (_screenMenus = _menuService.GetScreenMenus()); }
            set { _screenMenus = value; }
        }

        public int ScreenMenuId { get { return Model.ScreenMenuId; } set { Model.ScreenMenuId = value; } }
       
        private IEnumerable<Numerator> _numerators;
        public IEnumerable<Numerator> Numerators { get { return _numerators ?? (_numerators = Workspace.All<Numerator>()); } set { _numerators = value; } }

        public Numerator TicketNumerator { get { return Model.TicketNumerator; } set { Model.TicketNumerator = value; } }
        public Numerator OrderNumerator { get { return Model.OrderNumerator; } set { Model.OrderNumerator = value; } }

        private IEnumerable<AccountTransactionType> _accountTransactionTypes;
        public IEnumerable<AccountTransactionType> AccountTransactionTypes { get { return _accountTransactionTypes ?? (_accountTransactionTypes = Workspace.All<AccountTransactionType>()); } }

        public AccountTransactionType SaleTransactionType { get { return Model.SaleTransactionType; } set { Model.SaleTransactionType = value; } }

        public override string GetModelTypeString()
        {
            return Resources.TicketType;
        }

        public override Type GetViewType()
        {
            return typeof(TicketTypeView);
        }

        protected override AbstractValidator<TicketType> GetValidator()
        {
            return new TicketTypeValidator();
        }
    }

    internal class TicketTypeValidator : EntityValidator<TicketType>
    {
        public TicketTypeValidator()
        {
            RuleFor(x => x.TicketNumerator).NotNull();
            RuleFor(x => x.OrderNumerator).NotNull();
            RuleFor(x => x.SaleTransactionType).NotNull();
            RuleFor(x => x.SaleTransactionType.DefaultSourceAccountId).GreaterThan(0).When(x => x.SaleTransactionType != null);
            RuleFor(x => x.TicketNumerator).NotEqual(x => x.OrderNumerator).When(x => x.TicketNumerator != null);
        }
    }
}