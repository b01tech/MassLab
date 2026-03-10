import { UserRole } from './user-role';

export interface TokenPayload {
  sub?: string;
  nameid?: string;
  unique_name?: string;
  role?: UserRole;
  nbf: number;
  exp: number;
  iat: number;
  iss: string;
  aud: string;
}
