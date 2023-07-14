import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersComponent } from './users/users.component';
import { RootComponent } from './root/root.component';
import { SharedMaterialModule } from '../shared-material/shared-material.module';
import { RouterModule } from '@angular/router';
import { routing } from './admin.routing';
import { AddUserComponent } from './users/add-user/add-user.component';
import { SettingsComponent } from './settings/settings.component';
import { DashboardComponent } from './dashboard/dashboard.component';




@NgModule({
  declarations: [
    UsersComponent,
    RootComponent,
    AddUserComponent,
    SettingsComponent,
    DashboardComponent,
  ],
  imports: [
    CommonModule,
    SharedMaterialModule,
    routing
  ], exports: [SharedMaterialModule]
})
export class AdminModule { }
