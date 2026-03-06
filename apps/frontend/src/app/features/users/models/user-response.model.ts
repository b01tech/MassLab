import { User } from '../../auth/models/user.model';

export interface Pagination {
  page: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
}

export interface PaginatedUserResponse {
  pagination: Pagination;
  data: User[];
}
