import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WordFormsPageComponent } from './pages/word-forms-page/word-forms-page.component';

const routes: Routes = [
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
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
