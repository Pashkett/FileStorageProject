import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';

import { AppComponent } from './app.component';
import { from } from 'rxjs';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { AnimationBuilder } from '@angular/animations';
import { RecycleBinComponent } from './recycle-bin/recycle-bin.component';
import { StorageItemsComponent } from './storage-items/storage-items.component';
import { fileStorageRoutes } from './routes';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { ContainsRoleDirective } from './_directives/contains-role.directive';
import { UserManagementComponent } from './admin/user-management/user-management.component';
import { FileManagementComponent } from './admin/file-management/file-management.component';
import { AdminService } from './_services/admin.service';


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
      StorageItemsComponent,
      AdminPanelComponent,
      ContainsRoleDirective,
      UserManagementComponent,
      FileManagementComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      BrowserAnimationsModule,
      BsDropdownModule.forRoot(),
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
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
