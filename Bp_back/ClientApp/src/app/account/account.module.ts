import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { SharedMaterialModule } from '../shared-material/shared-material.module';
import { routing } from './account.routing';



@NgModule({
  declarations: [
    LoginComponent
  ],
  imports: [
    CommonModule,
    SharedMaterialModule,
    routing
  ]
})
export class AccountModule { }
