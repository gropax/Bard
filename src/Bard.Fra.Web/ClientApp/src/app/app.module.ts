import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http'

import { BrowserModule } from '@angular/platform-browser';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list'
import { MatFormFieldModule } from '@angular/material/form-field'
import { MatInputModule } from '@angular/material/input'
import { MatAutocompleteModule } from '@angular/material/autocomplete'
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { WordFormsPageComponent } from './pages/word-forms-page/word-forms-page.component';
import { WordFormsService } from './services/word-forms-service';
import { WordFormSelectorComponent } from './components/word-form-selector/word-form-selector.component';

@NgModule({
  declarations: [
    AppComponent,
    WordFormsPageComponent,
    WordFormSelectorComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,

    MatIconModule,
    MatListModule,
    MatFormFieldModule,
    MatButtonModule,
    MatToolbarModule,
    MatSidenavModule,
    MatAutocompleteModule,
    MatInputModule,
    MatProgressSpinnerModule,

    AppRoutingModule,
    BrowserAnimationsModule
  ],
  providers: [
    WordFormsService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
