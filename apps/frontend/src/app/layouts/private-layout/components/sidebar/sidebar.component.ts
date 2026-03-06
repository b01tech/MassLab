import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './sidebar.component.html',
})
export class SidebarComponent {
  navItems = [
    { label: 'Dashboard', icon: 'dashboard', route: '/dashboard', exact: true },
    { label: 'Usuários', icon: 'people', route: '/dashboard/users', exact: false },
    { label: 'Emissor', icon: 'business_center', route: '/dashboard/issuer', exact: false },
    { label: 'Clientes', icon: 'groups', route: '/dashboard/clients', exact: false },
    { label: 'Padrões', icon: 'straighten', route: '/dashboard/standards', exact: false },
    { label: 'Ordem de serviço', icon: 'assignment', route: '/dashboard/service-orders', exact: false },
    { label: 'Calibração', icon: 'science', route: '/dashboard/calibration', exact: false },
    { label: 'Relatórios', icon: 'analytics', route: '/dashboard/reports', exact: false },
    { label: 'Auditoria', icon: 'verified_user', route: '/dashboard/audit', exact: false },
  ];
}
