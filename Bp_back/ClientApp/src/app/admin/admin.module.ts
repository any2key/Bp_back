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
import { HubsComponent } from './hubs/hubs.component';
import { ServersComponent } from './servers/servers.component';
import { PlayersComponent } from './players/players.component';
import { AddHubComponent } from './hubs/add-hub/add-hub.component';




@NgModule({
  declarations: [
    UsersComponent,
    RootComponent,
    AddUserComponent,
    SettingsComponent,
    DashboardComponent,
    HubsComponent,
    ServersComponent,
    PlayersComponent,
    AddHubComponent,
  ],
  imports: [
    CommonModule,
    SharedMaterialModule,
    routing
  ], exports: [SharedMaterialModule]
})
export class AdminModule { }
