import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { HomeComponent } from './components/home/home.component';
import { ErrorComponent } from './components/error/error.component';
import { SuccessComponent } from './components/success/success.component';

import { TransactionService } from './components/services/transaction.service';

@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        ErrorComponent,
        SuccessComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'error', component: ErrorComponent },
            { path: 'success', component: SuccessComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [
        TransactionService
    ]
})
export class AppModuleShared {
}
