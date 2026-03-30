export enum AllergenEnum {
    Gluten = 'Glutén',
    Milk = 'Tej',
    Egg = 'Tojás',
    Fish = 'Hal',
    Peanut = 'Földimogyoró',
    Soy = 'Szójabab',
    TreeNuts = 'Diófélék',
    Celery = 'Zeller',
    Mustard = 'Mustár',
    Sesame = 'Szezámmag',
    SulphurDioxide = 'Kén-dioxid',
    Lupin = 'Csillagfürt',
    Molluscs = 'Puhatestűek',
    Crustaceans = 'Rákfélék',
}

export const allergenList = [
    'Glutén',
    'Tej',
    'Tojás',
    'Hal',
    'Földimogyoró',
    'Szójabab',
    'Diófélék',
    'Zeller',
    'Mustár',
    'Szezámmag',
    'Kén-dioxid',
    'Csillagfürt',
    'Puhatestűek',
    'Rákfélék',
] as const

export type Allergen = (typeof allergenList)[number]
