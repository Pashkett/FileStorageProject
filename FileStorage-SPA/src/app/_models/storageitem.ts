import { User } from './user';

export interface StorageItem {
    id?: string;
    displayName: string;
    extension: string;
    size: number;
    isFolder: boolean;
    isRecycled: boolean;
    isPublic: boolean;
    user: User;
    userId: string;
    createdOn: Date;
}
