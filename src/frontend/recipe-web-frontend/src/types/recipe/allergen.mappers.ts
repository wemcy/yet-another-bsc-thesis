import { Allergen } from 'recipe-api-client'
import { AllergenEnum } from './allergens'

export function MapApiAllergenToEnum(allergen: Allergen): AllergenEnum {
    switch (allergen) {
        case Allergen.Gluten:
            return AllergenEnum.Gluten
        case Allergen.Milk:
            return AllergenEnum.Milk
        case Allergen.Eggs:
            return AllergenEnum.Egg
        case Allergen.Fish:
            return AllergenEnum.Fish
        case Allergen.Peanuts:
            return AllergenEnum.Peanut
        case Allergen.Soybeans:
            return AllergenEnum.Soy
        case Allergen.Nuts:
            return AllergenEnum.TreeNuts
        case Allergen.Celery:
            return AllergenEnum.Celery
        case Allergen.Mustard:
            return AllergenEnum.Mustard
        case Allergen.Sesameseeds:
            return AllergenEnum.Sesame
        case Allergen.Sulphurdioxide:
            return AllergenEnum.SulphurDioxide
        case Allergen.Lupin:
            return AllergenEnum.Lupin
        case Allergen.Molluscs:
            return AllergenEnum.Molluscs
        case Allergen.Crustaceans:
            return AllergenEnum.Crustaceans
        default:
            throw new Error(`Unknown allergen: ${allergen}`)
    }
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
        case AllergenEnum.Crustaceans:
            return 'CRUSTACEANS'
    }
}
