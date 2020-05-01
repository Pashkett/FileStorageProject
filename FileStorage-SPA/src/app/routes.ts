import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { StorageItemsComponent } from './storage-items/storage-items.component';
import { RecycleBinComponent } from './recycle-bin/recycle-bin.component';
import { AuthGuard } from './_guards/auth.guard';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';

export const fileStorageRoutes: Routes = [
    { path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'storage-items', component: StorageItemsComponent },
            { path: 'recycle-bin', component: RecycleBinComponent },
            { path: 'admin', component: AdminPanelComponent, data: {roles: ['Admin', 'Moderator']}}
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' }
];
