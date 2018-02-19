import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl, ValidatorFn, FormArray } from '@angular/forms';
import { Router } from '@angular/router';

import 'rxjs/add/operator/debounceTime';

import { ITransaction } from '../services/Transaction';
import { TransactionService } from '../services/transaction.service';



@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {
    transactionForm: FormGroup;
    transaction: ITransaction;
    errorMessage: string;
    accountNumberPatter: "[0-9]{8}";
    bsbPattern: "[0-9]{6}"

    constructor(private fb: FormBuilder, private transactionService: TransactionService, private router: Router) { }

    ngOnInit(): void {
        this.transactionForm = this.fb.group({
            firstName: ['', [Validators.required, Validators.minLength(3)]],
            lastName: ['', [Validators.required, Validators.maxLength(50)]],
            accountNumber: ['', [Validators.required, Validators.minLength(7),Validators.maxLength(8)]],
            bsb: ['', [Validators.required, Validators.minLength(5),Validators.maxLength(6)]],
            amount: ['', [Validators.required, Validators.min(10), Validators.max(20001), Validators.maxLength(5)]]
        });

    }

    populateTestData(): void {
        this.transactionForm.patchValue({
            firstName: 'Jack',
            lastName: 'Harkness',
            accountNumber: '12345678',
            bsb: '123456',
            amount: '20000'
        });
    }

    save(): void {
        console.log('Saved: ' + JSON.stringify(this.transactionForm.value));
        if (this.transactionForm.dirty && this.transactionForm.valid) {
            // Copy the form values over the trasactionForm object values
            let p = Object.assign({}, this.transaction, this.transactionForm.value);

            this.transactionService.saveTransaction(p)
                .subscribe(
                (data: any) => this.onSaveComplete(data),
                (error: any) => this.router.navigate(['/error'])
                );
        }
        else if (!this.transactionForm.dirty) {
            this.transactionForm.reset();
        }
    }

    onSaveComplete(data: any): void {
        // Reset the form to clear the flags
        this.transactionForm.reset();
        if (data.transactionStatus && data.transactionStatus === 'Success')
            this.router.navigate(['/success']);
        else
            this.router.navigate(['/error']);
    }
}
