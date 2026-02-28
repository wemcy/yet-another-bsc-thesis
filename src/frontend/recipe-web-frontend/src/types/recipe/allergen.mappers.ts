import type { Allergen } from 'recipe-api-client'
import { AllergenEnum } from './allergens'

export function MapApiAllergenToEnum(allergen: string): AllergenEnum | null {
    switch (allergen) {
        case 'Glutén':
            return AllergenEnum.Gluten
        case 'Tej':
            return AllergenEnum.Milk
        case 'Tojás':
            return AllergenEnum.Egg
        case 'Hal':
            return AllergenEnum.Fish
        case 'Földimogyoró':
            return AllergenEnum.Peanut
        case 'Szójabab':
            return AllergenEnum.Soy
        case 'Diófélék':
            return AllergenEnum.TreeNuts
        case 'Zeller':
            return AllergenEnum.Celery
        case 'Mustár':
            return AllergenEnum.Mustard
        case 'Szezámmag':
            return AllergenEnum.Sesame
        case 'Kén-dioxid':
            return AllergenEnum.SulphurDioxide
        case 'Csillagfürt':
            return AllergenEnum.Lupin
        case 'Puhatestűek':
            return AllergenEnum.Molluscs
    }
    return null
}

export function MapEnumToApiAllergen(allergen: AllergenEnum): Allergen {
    switch (allergen) {
        case AllergenEnum.Gluten:
            return 'GLUTEN'
        case AllergenEnum.Milk:
            return 'MILK'
        case AllergenEnum.Egg:
            return 'EGGS'
        case AllergenEnum.Fish:
            return 'FISH'
        case AllergenEnum.Peanut:
            return 'PEANUTS'
        case AllergenEnum.Soy:
            return 'SOYBEANS'
        case AllergenEnum.TreeNuts:
            return 'NUTS'
        case AllergenEnum.Celery:
            return 'CELERY'
        case AllergenEnum.Mustard:
            return 'MUSTARD'
        case AllergenEnum.Sesame:
            return 'SESAMESEEDS'
        case AllergenEnum.SulphurDioxide:
            return 'SULPHURDIOXIDE'
        case AllergenEnum.Lupin:
            return 'LUPIN'
        case AllergenEnum.Molluscs:
            return 'MOLLUSCS'
    }
}
