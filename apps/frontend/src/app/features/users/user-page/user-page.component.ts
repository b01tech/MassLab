import { Component, computed, inject } from '@angular/core';
import { UserListComponent } from '../components/user-list/user-list.component';
import { UserHeaderComponent } from '../components/user-header/user-header.component';
import { AuthService } from '../../auth/services/auth.service';
import { UserRole } from '../../auth/models/user-role';

@Component({
  selector: 'app-user-page',
  standalone: true,
  templateUrl: './user-page.component.html',
  imports: [UserListComponent, UserHeaderComponent],
})
export class UserPageComponent {
  private readonly authService = inject(AuthService);

  readonly loggedUser = this.authService.loggedUser;

  readonly canAccessUserList = computed(() => {
    const role = this.loggedUser()?.role;
    return role === UserRole.Admin || role === UserRole.Manager;
  });
}
