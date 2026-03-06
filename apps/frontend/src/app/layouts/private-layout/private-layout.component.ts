import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { PrivateHeaderComponent } from './components/header/header.component';

@Component({
  selector: 'app-private-layout',
  standalone: true,
  imports: [RouterOutlet, SidebarComponent, PrivateHeaderComponent],
  templateUrl: './private-layout.component.html',
})
export class PrivateLayoutComponent {}
