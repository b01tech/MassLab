import { Component, EventEmitter, Input, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { User } from '../../../auth/models/user.model';
import { UserRole } from '../../../auth/models/user-role';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    @if (isOpen) {
      <div class="fixed inset-0 z-50 flex items-center justify-center bg-black/50 backdrop-blur-sm">
        <div class="bg-white rounded-lg shadow-xl w-full max-w-md p-6 transform transition-all scale-100">
          <h2 class="text-xl font-bold text-gray-800 mb-4">
            {{ isEditMode ? 'Editar Usuário' : 'Novo Usuário' }}
          </h2>
          
          <form [formGroup]="userForm" (ngSubmit)="onSubmit()" class="space-y-4">
            <!-- Nome de Usuário -->
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Nome de Usuário</label>
              <input 
                type="text" 
                formControlName="userName"
                class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                placeholder="Digite o nome de usuário"
              />
              @if (userForm.get('userName')?.invalid && userForm.get('userName')?.touched) {
                <div class="text-red-500 text-xs mt-1">
                  Nome de usuário é obrigatório.
                </div>
              }
            </div>

            <!-- Cargo -->
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Cargo</label>
              <select 
                formControlName="role"
                class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 bg-white cursor-pointer"
              >
                @for (role of roles; track role) {
                  <option [value]="role">{{ getRoleLabel(role) }}</option>
                }
              </select>
            </div>

            <!-- Senha (apenas para novos usuários ou alteração opcional) -->
            @if (!isEditMode) {
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Senha</label>
                <input 
                  type="password" 
                  formControlName="password"
                  class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                  placeholder="Digite a senha"
                />
                @if (userForm.get('password')?.invalid && userForm.get('password')?.touched) {
                  <div class="text-red-500 text-xs mt-1">
                    Senha é obrigatória (mínimo 6 caracteres).
                  </div>
                }
              </div>
            }

            <!-- Actions -->
            <div class="flex justify-end space-x-3 mt-6">
              <button 
                type="button" 
                (click)="close.emit()"
                class="cursor-pointer px-4 py-2 border border-gray-300 rounded-md text-sm font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
              >
                Cancelar
              </button>
              <button 
                type="submit" 
                [disabled]="userForm.invalid"
                class="cursor-pointer px-4 py-2 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed"
              >
                Salvar
              </button>
            </div>
          </form>
        </div>
      </div>
    }
  `
})
export class UserFormComponent {
  @Input() isOpen = false;
  @Input() set user(value: User | null) {
    if (value) {
      this.isEditMode = true;
      this.userForm.patchValue({
        userName: value.userName,
        role: value.role
      });
      this.userForm.get('password')?.clearValidators();
    } else {
      this.isEditMode = false;
      this.userForm.reset({ role: UserRole.Operator });
      this.userForm.get('password')?.setValidators([Validators.required, Validators.minLength(6)]);
    }
    this.userForm.get('password')?.updateValueAndValidity();
  }

  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<any>();

  fb = inject(FormBuilder);
  isEditMode = false;
  roles = Object.values(UserRole);

  userForm: FormGroup = this.fb.group({
    userName: ['', Validators.required],
    role: [UserRole.Operator, Validators.required],
    password: ['']
  });

  getRoleLabel(role: UserRole): string {
    switch (role) {
      case UserRole.Admin: return 'Administrador';
      case UserRole.Manager: return 'Gerente';
      case UserRole.Operator: return 'Técnico';
      default: return role;
    }
  }

  onSubmit() {
    if (this.userForm.valid) {
      this.save.emit(this.userForm.value);
    }
  }
}
