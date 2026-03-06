import { UserRole } from './user-role';

export interface User {
  id: string;
  username: string;
  role: UserRole;
}
