import { ModuleWithProviders } from '@angular/core';
import { RouterModule } from '@angular/router';

import { LoginComponent } from './login/login.component';

export const routing: ModuleWithProviders<any> = RouterModule.forChild([
  { path: 'login', component: LoginComponent },
]);
