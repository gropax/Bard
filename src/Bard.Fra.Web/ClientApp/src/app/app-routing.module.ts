import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EditorPageComponent } from './pages/editor-page/editor-page.component';
import { RhymesPageComponent } from './pages/rhymes-page/rhymes-page.component';
import { WordFormsPageComponent } from './pages/word-forms-page/word-forms-page.component';

const routes: Routes = [
  {
    path: 'editor',
    component: EditorPageComponent,
  },
  {
    path: 'word-forms',
    children: [
      { path: '', component: WordFormsPageComponent, },
    //  {
    //    path: ':word-forms-id',
    //    resolve: { catalog: CatalogResolver, },
    //    children: [
    //      { path: '', component: CatalogPage, },
    //      {
    //        path: 'blocks/:blockGuid',
    //        component: BlockPage,
    //        resolve: { block: BlockResolver },
    //      },
    //    ]
    //  },
    ],
  },
  {
    path: 'rhymes',
    component: RhymesPageComponent
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
