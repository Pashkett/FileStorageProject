import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';
import { FileUploadModule } from 'ng2-file-upload';

import { AppComponent } from './app.component';
import { from } from 'rxjs';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { AnimationBuilder } from '@angular/animations';
import { RecycleBinComponent } from './recycle-bin/recycle-bin.component';
import { fileStorageRoutes } from './routes';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { ContainsRoleDirective } from './_directives/contains-role.directive';
import { UserManagementComponent } from './admin/user-management/user-management.component';
import { AdminService } from './_services/admin.service';
import { RolesModalComponent } from './admin/roles-modal/roles-modal.component';
import { SharedItemsComponent } from './shared-items/shared-items.component';
import { StorageTabsComponent } from './storage-general/storage-tabs/storage-tabs.component';
import { StorageItemsComponent } from './storage-general/storage-items/storage-items.component';
import { StorageUploadComponent } from './storage-general/storage-upload/storage-upload.component';
import { ModerateItemsComponent } from './moderate-items/moderate-items.component';


export function tokenGetter() {
   return localStorage.getItem('token');
}

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      RecycleBinComponent,
      StorageTabsComponent,
      StorageItemsComponent,
      StorageUploadComponent,
      AdminPanelComponent,
      ContainsRoleDirective,
      UserManagementComponent,
      RolesModalComponent,
      SharedItemsComponent,
      ModerateItemsComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      BrowserAnimationsModule,
      BsDropdownModule.forRoot(),
      ModalModule.forRoot(),
      TabsModule.forRoot(),
      ButtonsModule.forRoot(),
      PaginationModule.forRoot(),
      CollapseModule.forRoot(),
      FileUploadModule,
      RouterModule.forRoot(fileStorageRoutes),
      JwtModule.forRoot({
         config: {
            tokenGetter,
            whitelistedDomains: ['localhost:5001', 'localhost:5000'],
            blacklistedRoutes: ['localhost:5001/api/auth', 'localhost:5000/api/auth']
         }
      })
   ],
   providers: [
      AuthService,
      ErrorInterceptorProvider,
      AdminService
   ],
   entryComponents: [
      RolesModalComponent
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
