import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router'

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { AuthInterceptorProvider } from './Interceptors/auth.Interceptor';
import { NotFoundComponent } from './not-found/not-found.component';
import { ForbiddenComponent } from './forbidden/forbidden.component';
import { ErrorInterceptorProvider } from './Interceptors/error.Interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SuccessModalComponent } from './success-modal/success-modal.component';



import { ConfirmComponent } from './confirm/confirm.component';
import { routing } from './app.routing';
import { AccountModule } from './account/account.module';
import { SharedMaterialModule } from './shared-material/shared-material.module';
import { AdminModule } from './admin/admin.module';



@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NotFoundComponent,
    ForbiddenComponent,
    ConfirmComponent,
    SuccessModalComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    routing,
    ReactiveFormsModule,
    AccountModule,
    SharedMaterialModule,
    BrowserAnimationsModule,
    AdminModule,
  ],
  providers: [
    AuthInterceptorProvider,
    ErrorInterceptorProvider
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
