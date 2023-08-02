import { ModuleWithProviders } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AdminAuthGuard } from '../guards/adminAuth.guard';
import { DashboardComponent } from './dashboard/dashboard.component';
import { HubsComponent } from './hubs/hubs.component';
import { PlayersComponent } from './players/players.component';

import { RootComponent } from './root/root.component';
import { ServersComponent } from './servers/servers.component';
import { SettingsComponent } from './settings/settings.component';
import { UsersComponent } from './users/users.component';



export const routing: ModuleWithProviders<any> = RouterModule.forChild([
  {
    path: 'admin',
    component: RootComponent, canActivate: [AdminAuthGuard],

    children: [
      { path: '', component: DashboardComponent },
      { path: 'home', component: DashboardComponent },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'users', component: UsersComponent },
      { path: 'settings', component: SettingsComponent },
      { path: 'hubs', component: HubsComponent },
      { path: 'servers', component: ServersComponent },
      { path: 'servers/:Id', component: ServersComponent },
      { path: 'players', component: PlayersComponent },
    ]
  }
]);
