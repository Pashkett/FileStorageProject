import { Role } from './role';

export interface User {
    id: string;
    userName: string;
    roles?: Role[];
}
