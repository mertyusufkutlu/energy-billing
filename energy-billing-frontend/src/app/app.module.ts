import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BillingResultComponent } from './components/billing-result/billing-result.component';

@NgModule({
  declarations: [
    AppComponent,
    BillingResultComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    TableModule,
    ButtonModule,
    DropdownModule,
    InputTextModule,
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
