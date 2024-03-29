import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http'

import { BrowserModule } from '@angular/platform-browser';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list'
import { MatFormFieldModule } from '@angular/material/form-field'
import { MatInputModule } from '@angular/material/input'
import { MatAutocompleteModule } from '@angular/material/autocomplete'
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner'
import { MatChipsModule } from '@angular/material/chips'
import { MatTableModule } from '@angular/material/table'
import { MatPaginatorModule } from '@angular/material/paginator'
import { MatSortModule } from '@angular/material/sort'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { WordFormsPageComponent } from './pages/word-forms-page/word-forms-page.component';
import { WordFormsService } from './services/word-forms-service';
import { WordFormSelectorComponent } from './components/word-form-selector/word-form-selector.component';
import { PhonGraphWordSelectorComponent } from './components/phon-graph-word-selector/phon-graph-word-selector.component';
import { RhymesPageComponent } from './pages/rhymes-page/rhymes-page.component';
import { FinalRhymeTableComponent } from './pages/rhymes-page/final-rhyme-table/final-rhyme-table.component';
import { EditorPageComponent } from './pages/editor-page/editor-page.component';
import { PageContentComponent } from './components/page-content/page-content.component';
import { WordPartComponent } from './pages/editor-page/token-view/word-part/word-part.component';
import { ViewMenuButtonComponent } from './pages/editor-page/view-menu-button/view-menu-button.component';
import { TokenViewComponent } from './pages/editor-page/token-view/token-view.component';
import { IpaViewComponent } from './pages/editor-page/ipa-view/ipa-view.component';
import { SegmentComponent } from './pages/editor-page/ipa-view/segment/segment.component';

@NgModule({
  declarations: [
    AppComponent,
    WordFormsPageComponent,
    WordFormSelectorComponent,
    PhonGraphWordSelectorComponent,
    RhymesPageComponent,
    FinalRhymeTableComponent,
    EditorPageComponent,
    PageContentComponent,
    WordPartComponent,
    ViewMenuButtonComponent,
    TokenViewComponent,
    IpaViewComponent,
    SegmentComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,

    MatCardModule,
    MatIconModule,
    MatListModule,
    MatFormFieldModule,
    MatButtonModule,
    MatToolbarModule,
    MatSidenavModule,
    MatAutocompleteModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,

    AppRoutingModule,
    BrowserAnimationsModule
  ],
  providers: [
    WordFormsService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
