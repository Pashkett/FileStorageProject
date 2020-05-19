import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { RecycleBinComponent } from './recycle-bin/recycle-bin.component';
import { AuthGuard } from './_guards/auth.guard';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { SharedItemsComponent } from './shared-items/shared-items.component';
import { StorageTabsComponent } from './storage-general/storage-tabs/storage-tabs.component';
import { ModerateItemsComponent } from './moderate-items/moderate-items.component';

export const fileStorageRoutes: Routes = [
    { path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'storage-items', component: StorageTabsComponent },
            { path: 'shared-items', component: SharedItemsComponent },
            { path: 'recycle-bin', component: RecycleBinComponent },
            { path: 'admin', component: AdminPanelComponent, data: {roles: ['Admin']}},
            { path: 'moderate', component: ModerateItemsComponent, data: {roles: ['Moderator']}}
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' }
];
