import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '../services/user.service';
import { User } from '../../auth/models/user.model';
import { UserRole } from '../../auth/models/user-role';
import { UserFormComponent } from '../components/user-form/user-form.component';
import { FormsModule } from '@angular/forms';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, UserFormComponent, FormsModule],
  templateUrl: './user-list.component.html',
})
export class UserListComponent implements OnInit {
  protected Math = Math;
  private userService = inject(UserService);

  private searchSubject = new Subject<string>();

  users = this.userService.users;
  totalItems = this.userService.totalItems;

  currentPage = signal(1);
  pageSize = signal(10);
  searchTerm = signal('');

  isModalOpen = signal(false);
  selectedUser = signal<User | null>(null);
  errorMessage = signal<string | null>(null);

  ngOnInit() {
    this.loadUsers();

    // Debounce search
    this.searchSubject.pipe(debounceTime(300), distinctUntilChanged()).subscribe((term) => {
      this.searchTerm.set(term);
      this.currentPage.set(1); // Reset to first page on search
      this.loadUsers();
    });
  }

  loadUsers() {
    this.userService.getUsers(this.currentPage(), this.pageSize(), this.searchTerm()).subscribe();
  }

  onSearch(term: string) {
    this.searchSubject.next(term);
  }

  openCreateModal() {
    this.selectedUser.set(null);
    this.errorMessage.set(null);
    this.isModalOpen.set(true);
  }

  openEditModal(user: User) {
    this.selectedUser.set(user);
    this.errorMessage.set(null);
    this.isModalOpen.set(true);
  }

  closeModal() {
    this.isModalOpen.set(false);
    this.selectedUser.set(null);
  }

  saveUser(userData: any) {
    this.errorMessage.set(null);
    if (this.selectedUser()) {
      const payload = { ...userData, userId: this.selectedUser()!.id };
      this.userService.updateUser(this.selectedUser()!.id, payload).subscribe({
        next: () => {
          this.loadUsers();
          this.closeModal();
        },
        error: (err) => {
          console.error('Error updating user', err);
          this.handleError(err);
        },
      });
    } else {
      // Create
      this.userService.createUser(userData).subscribe({
        next: () => {
          this.loadUsers();
          this.closeModal();
        },
        error: (err) => {
          console.error('Error creating user', err);
          this.handleError(err);
        },
      });
    }
  }

  private handleError(err: any) {
    if (Array.isArray(err.error)) {
      this.errorMessage.set(err.error.join(', '));
    } else if (err.error?.message) {
      this.errorMessage.set(err.error.message);
    } else {
      this.errorMessage.set('Ocorreu um erro inesperado.');
    }
  }

  toggleStatus(user: User) {
    if (user.active) {
      this.userService.deactivateUser(user.id).subscribe(() => this.loadUsers());
    } else {
      this.userService.activateUser(user.id).subscribe(() => this.loadUsers());
    }
  }

  getRoleBadgeClass(role: UserRole): string {
    switch (role) {
      case UserRole.Admin:
        return 'bg-purple-100 text-purple-800 dark:bg-purple-900 dark:text-purple-300';
      case UserRole.Manager:
        return 'bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-300';
      case UserRole.Operator:
        return 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300';
      default:
        return 'bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300';
    }
  }

  getRoleLabel(role: UserRole): string {
    switch (role) {
      case UserRole.Admin:
        return 'Administrador';
      case UserRole.Manager:
        return 'Gerente';
      case UserRole.Operator:
        return 'Técnico';
      default:
        return role;
    }
  }

  // Pagination helpers
  nextPage() {
    if (this.currentPage() * this.pageSize() < this.totalItems()) {
      this.currentPage.update((p) => p + 1);
      this.loadUsers();
    }
  }

  prevPage() {
    if (this.currentPage() > 1) {
      this.currentPage.update((p) => p - 1);
      this.loadUsers();
    }
  }
}
