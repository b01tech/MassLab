import { UserRole } from './user-role';

export interface User {
  id: string;
  userName: string;
  role: UserRole;
  active: boolean;
}
