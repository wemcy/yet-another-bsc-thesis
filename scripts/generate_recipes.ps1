# Generate 100 recipe API calls for Bruno
# This script creates individual Bruno request files for 100 different recipes

$recipeDir = "f:\Programming\yet-another-bsc-thesis\docs\bruno\api-calls\recipes"
New-Item -ItemType Directory -Force -Path $recipeDir | Out-Null

# Diverse recipe data
$recipes = @(
    @{ title = "Spaghetti Carbonara"; desc = "Classic Italian pasta with eggs and bacon"; allergens = @(3, 7); steps = @("Boil pasta", "Fry bacon", "Mix eggs", "Combine"); ingredients = @(@{name="Spaghetti"; qty=400; unit="g"}, @{name="Bacon"; qty=200; unit="g"}, @{name="Eggs"; qty=4; unit="pcs"}) },
    @{ title = "Margherita Pizza"; desc = "Traditional Italian pizza"; allergens = @(7); steps = @("Prepare dough", "Add sauce", "Top with mozzarella", "Bake"); ingredients = @(@{name="Flour"; qty=500; unit="g"}, @{name="Mozzarella"; qty=250; unit="g"}) },
    @{ title = "Thai Green Curry"; desc = "Spicy Thai curry with coconut milk"; allergens = @(2); steps = @("Fry curry paste", "Add coconut milk", "Add vegetables", "Simmer"); ingredients = @(@{name="Coconut milk"; qty=400; unit="ml"}, @{name="Green curry paste"; qty=3; unit="tbsp"}) },
    @{ title = "Pad Thai"; desc = "Stir-fried noodles Thai style"; allergens = @(5, 6); steps = @("Cook noodles", "Stir-fry ingredients", "Add sauce", "Garnish"); ingredients = @(@{name="Rice noodles"; qty=300; unit="g"}, @{name="Peanut butter"; qty=3; unit="tbsp"}) },
    @{ title = "Beef Tacos"; desc = "Traditional Mexican tacos"; allergens = @(); steps = @("Brown beef", "Season", "Warm tortillas", "Assemble"); ingredients = @(@{name="Ground beef"; qty=500; unit="g"}, @{name="Tortillas"; qty=8; unit="pcs"}) },
    @{ title = "Fish and Chips"; desc = "Crispy battered fish with chips"; allergens = @(1, 3, 4); steps = @("Prepare batter", "Fry fish", "Fry chips", "Serve with sauce"); ingredients = @(@{name="White fish"; qty=600; unit="g"}, @{name="Potatoes"; qty=800; unit="g"}) },
    @{ title = "Chicken Tikka Masala"; desc = "Indian spiced chicken in creamy sauce"; allergens = @(7); steps = @("Marinate chicken", "Grill", "Make sauce", "Simmer"); ingredients = @(@{name="Chicken"; qty=800; unit="g"}, @{name="Yogurt"; qty=200; unit="ml"}) },
    @{ title = "Risotto Milanese"; desc = "Creamy saffron rice"; allergens = @(7); steps = @("Toast rice", "Add broth gradually", "Stir constantly", "Add butter and cheese"); ingredients = @(@{name="Arborio rice"; qty=300; unit="g"}, @{name="Saffron"; qty=1; unit="g"}) },
    @{ title = "Falafel Wrap"; desc = "Chickpea fritters in pita"; allergens = @(1, 6, 9); steps = @("Blend chickpeas", "Form patties", "Fry", "Wrap with vegetables"); ingredients = @(@{name="Chickpeas"; qty=400; unit="g"}, @{name="Pita bread"; qty=4; unit="pcs"}) },
    @{ title = "Beef Stew"; desc = "Hearty beef and vegetable stew"; allergens = @(); steps = @("Brown beef", "Add vegetables", "Simmer slowly", "Season to taste"); ingredients = @(@{name="Beef chuck"; qty=1000; unit="g"}, @{name="Carrots"; qty=300; unit="g"}) },
    @{ title = "Greek Salad"; desc = "Fresh Mediterranean salad"; allergens = @(7); steps = @("Chop vegetables", "Add feta cheese", "Mix olive oil and vinegar", "Toss"); ingredients = @(@{name="Tomatoes"; qty=400; unit="g"}, @{name="Feta cheese"; qty=200; unit="g"}) },
    @{ title = "Sushi Rolls"; desc = "Japanese rice and fish rolls"; allergens = @(4); steps = @("Cook rice", "Prepare fillings", "Roll on mat", "Slice"); ingredients = @(@{name="Sushi rice"; qty=300; unit="g"}, @{name="Salmon"; qty=200; unit="g"}) },
    @{ title = "Lemon Chicken"; desc = "Tangy lemon glazed chicken"; allergens = @(); steps = @("Pan-fry chicken", "Make lemon sauce", "Combine", "Garnish"); ingredients = @(@{name="Chicken breasts"; qty=800; unit="g"}, @{name="Lemons"; qty=3; unit="pcs"}) },
    @{ title = "Vegetable Stir-Fry"; desc = "Quick Asian vegetables"; allergens = @(6); steps = @("Chop vegetables", "Heat wok", "Stir-fry", "Add soy sauce"); ingredients = @(@{name="Broccoli"; qty=300; unit="g"}, @{name="Soy sauce"; qty=3; unit="tbsp"}) },
    @{ title = "French Onion Soup"; desc = "Caramelized onion soup with cheese"; allergens = @(1, 7); steps = @("Caramelize onions", "Add broth", "Simmer", "Top with bread and cheese"); ingredients = @(@{name="Onions"; qty=1000; unit="g"}, @{name="Gruyere cheese"; qty=200; unit="g"}) },
    @{ title = "Pesto Pasta"; desc = "Fresh basil pasta"; allergens = @(7, 8); steps = @("Make pesto", "Cook pasta", "Mix", "Serve hot"); ingredients = @(@{name="Basil"; qty=100; unit="g"}, @{name="Pine nuts"; qty=50; unit="g"}) },
    @{ title = "Beef Burgers"; desc = "Juicy homemade burgers"; allergens = @(1); steps = @("Form patties", "Season", "Grill", "Top with toppings"); ingredients = @(@{name="Ground beef"; qty=400; unit="g"}, @{name="Burger buns"; qty=4; unit="pcs"}) },
    @{ title = "Mushroom Risotto"; desc = "Creamy mushroom rice"; allergens = @(7); steps = @("Sauté mushrooms", "Toast rice", "Add broth", "Stir"); ingredients = @(@{name="Mushrooms"; qty=500; unit="g"}, @{name="Arborio rice"; qty=300; unit="g"}) },
    @{ title = "Teriyaki Salmon"; desc = "Sweet glazed salmon"; allergens = @(4, 6); steps = @("Make glaze", "Pan-sear salmon", "Glaze", "Serve with rice"); ingredients = @(@{name="Salmon fillet"; qty=600; unit="g"}, @{name="Soy sauce"; qty=4; unit="tbsp"}) },
    @{ title = "Vegetable Curry"; desc = "Mild Indian vegetable curry"; allergens = @(); steps = @("Sauté onions", "Add spices", "Add vegetables", "Simmer"); ingredients = @(@{name="Mixed vegetables"; qty=800; unit="g"}, @{name="Curry powder"; qty=2; unit="tbsp"}) },
    @{ title = "Chicken Pasta Alfredo"; desc = "Creamy Italian pasta with chicken"; allergens = @(1, 3, 7); steps = @("Cook pasta", "Fry chicken", "Make cream sauce", "Combine"); ingredients = @(@{name="Pasta"; qty=400; unit="g"}, @{name="Cream"; qty=300; unit="ml"}) },
    @{ title = "vegetable soup"; desc = "Healthy broiled vegetables"; allergens = @(); steps = @("Chop vegetables", "Boil", "Blend", "Season"); ingredients = @(@{name="Mixed veg"; qty=1000; unit="g"}, @{name="Vegetable broth"; qty=1000; unit="ml"}) },
    @{ title = "Enchiladas Verdes"; desc = "Green sauce chicken enchiladas"; allergens = @(1, 7); steps = @("Cook chicken", "Make green sauce", "Roll in tortillas", "Bake"); ingredients = @(@{name="Chicken"; qty=600; unit="g"}, @{name="Corn tortillas"; qty=12; unit="pcs"}) },
    @{ title = "Beef Wellington"; desc = "Beef wrapped in puff pastry"; allergens = @(1, 7); steps = @("Sear beef", "Add mushroom pâté", "Wrap in pastry", "Bake"); ingredients = @(@{name="Beef tenderloin"; qty=800; unit="g"}, @{name="Puff pastry"; qty=400; unit="g"}) },
    @{ title = "Shrimp Scampi"; desc = "Garlic butter shrimp"; allergens = @(2); steps = @("Heat butter and garlic", "Add shrimp", "Cook briefly", "Serve with pasta"); ingredients = @(@{name="Shrimp"; qty=500; unit="g"}, @{name="Garlic"; qty=6; unit="cloves"}) },
    @{ title = "Vegetable Lasagna"; desc = "Layered pasta with vegetables"; allergens = @(1, 3, 7); steps = @("Make sauce", "Layer pasta and sauce", "Bake", "Cool slightly"); ingredients = @(@{name="Lasagna sheets"; qty=300; unit="g"}, @{name="Spinach"; qty=400; unit="g"}) },
    @{ title = "Tandoori Chicken"; desc = "Indian spiced roasted chicken"; allergens = @(7); steps = @("Marinate in yogurt", "Rub with spices", "Grill", "Rest before serving"); ingredients = @(@{name="Chicken legs"; qty=1000; unit="g"}, @{name="Yogurt"; qty=200; unit="ml"}) },
    @{ title = "Beef Fajitas"; desc = "Sautéed beef strips with peppers"; allergens = @(); steps = @("Marinate beef", "Sauté vegetables", "Cook beef", "Serve with tortillas"); ingredients = @(@{name="Beef strips"; qty=600; unit="g"}, @{name="Bell peppers"; qty=400; unit="g"}) },
    @{ title = "Clam Linguine"; desc = "Fresh clams with pasta"; allergens = @(1, 10); steps = @("Cook pasta", "Sauté clams", "Make sauce", "Combine"); ingredients = @(@{name="Linguine"; qty=400; unit="g"}, @{name="Clams"; qty=800; unit="g"}) },
    @{ title = "Chickpea Curry"; desc = "Vegan Indian curry"; allergens = @(); steps = @("Toast spices", "Sauté onions", "Add chickpeas", "Simmer"); ingredients = @(@{name="Chickpeas"; qty=500; unit="g"}, @{name="Coconut milk"; qty=400; unit="ml"}) },
    @{ title = "Chicken Souvlaki"; desc = "Greek grilled chicken skewers"; allergens = @(9); steps = @("Marinate chicken", "Thread on skewers", "Grill", "Serve with pita"); ingredients = @(@{name="Chicken breast"; qty=800; unit="g"}, @{name="Olive oil"; qty=100; unit="ml"}) },
    @{ title = "Beef Tacos Al Pastor"; desc = "Mexican spiced beef tacos"; allergens = @(); steps = @("Marinate beef", "Cook", "Warm tortillas", "Assemble"); ingredients = @(@{name="Beef"; qty=600; unit="g"}, @{name="Corn tortillas"; qty=8; unit="pcs"}) },
    @{ title = "Mussels Marinière"; desc = "French mussels in white wine"; allergens = @(10); steps = @("Clean mussels", "Steam with wine", "Remove shells", "Serve with bread"); ingredients = @(@{name="Mussels"; qty=1000; unit="g"}, @{name="White wine"; qty=500; unit="ml"}) },
    @{ title = "Tofu Stir-Fry"; desc = "Asian tofu and vegetable stir-fry"; allergens = @(6); steps = @("Cube tofu", "Stir-fry tofu", "Add vegetables", "Add sauce"); ingredients = @(@{name="Tofu"; qty=600; unit="g"}, @{name="Mixed vegetables"; qty=400; unit="g"}) },
    @{ title = "Duck Confit"; desc = "Slow-cooked duck legs"; allergens = @(); steps = @("Season duck", "Cook in fat", "Cool slowly", "Reheat before serving"); ingredients = @(@{name="Duck legs"; qty=4; unit="pcs"}, @{name="Duck fat"; qty=250; unit="g"}) },
    @{ title = "Caprese Salad"; desc = "Fresh mozzarella and tomato"; allergens = @(7); steps = @("Slice tomatoes and mozzarella", "Layer", "Add basil", "Dress with oil"); ingredients = @(@{name="Tomatoes"; qty=400; unit="g"}, @{name="Mozzarella"; qty=250; unit="g"}) },
    @{ title = "Lamb Kebabs"; desc = "Grilled lamb and vegetable skewers"; allergens = @(9); steps = @("Cube lamb", "Thread on skewers", "Grill", "Serve with sauce"); ingredients = @(@{name="Lamb"; qty=800; unit="g"}, @{name="Bell peppers"; qty=300; unit="g"}) },
    @{ title = "Seafood Paella"; desc = "Spanish rice with seafood"; allergens = @(2, 4, 10); steps = @("Toast rice", "Add broth", "Add seafood", "Simmer"); ingredients = @(@{name="Paella rice"; qty=400; unit="g"}, @{name="Seafood mix"; qty=600; unit="g"}) },
    @{ title = "Vegetable Pie"; desc = "Savory vegetable pot pie"; allergens = @(1, 7); steps = @("Make filling", "Layer vegetables", "Top with pastry", "Bake"); ingredients = @(@{name="Mixed vegetables"; qty=800; unit="g"}, @{name="Puff pastry"; qty=300; unit="g"}) },
    @{ title = "Chicken Katsu"; desc = "Japanese breaded chicken"; allergens = @(1, 3, 4); steps = @("Bread chicken", "Fry", "Cut into strips", "Serve with sauce"); ingredients = @(@{name="Chicken breast"; qty=800; unit="g"}, @{name="Panko breadcrumbs"; qty=100; unit="g"}) },
    @{ title = "Beef Bourguignon"; desc = "Red wine braised beef"; allergens = @(); steps = @("Brown beef", "Add wine", "Braise slowly", "Add vegetables"); ingredients = @(@{name="Beef chuck"; qty=1200; unit="g"}, @{name="Red wine"; qty=750; unit="ml"}) },
    @{ title = "Vegetable Frittata"; desc = "Italian egg and vegetable bake"; allergens = @(3, 7); steps = @("Sauté vegetables", "Pour eggs", "Cook on stovetop", "Finish in oven"); ingredients = @(@{name="Eggs"; qty=8; unit="pcs"}, @{name="Mixed vegetables"; qty=400; unit="g"}) },
    @{ title = "Thai Basil Chicken"; desc = "Spicy Thai chicken with basil"; allergens = @(); steps = @("Fry chicken", "Add garlic", "Add basil and chilies", "Serve over rice"); ingredients = @(@{name="Chicken breast"; qty=800; unit="g"}, @{name="Thai basil"; qty=50; unit="g"}) },
    @{ title = "Salmon Cakes"; desc = "Pan-fried salmon cakes"; allergens = @(1, 3, 4); steps = @("Mix salmon", "Form patties", "Fry", "Serve with sauce"); ingredients = @(@{name="Salmon"; qty=500; unit="g"}, @{name="Breadcrumbs"; qty=100; unit="g"}) },
    @{ title = "Vegetable Moussaka"; desc = "Layered eggplant and vegetable"; allergens = @(3, 7); steps = @("Fry eggplant", "Make sauce", "Layer", "Bake"); ingredients = @(@{name="Eggplant"; qty=500; unit="g"}, @{name="Parmesan"; qty=150; unit="g"}) },
    @{ title = "Chicken Piccata"; desc = "Lemon caper chicken"; allergens = @(1); steps = @("Pound chicken", "Fry", "Make sauce with lemons", "Serve"); ingredients = @(@{name="Chicken breast"; qty=800; unit="g"}, @{name="Capers"; qty=50; unit="g"}) },
    @{ title = "Black Bean Tacos"; desc = "Vegetarian black bean tacos"; allergens = @(6); steps = @("Heat beans", "Warm tortillas", "Add toppings", "Serve"); ingredients = @(@{name="Black beans"; qty=400; unit="g"}, @{name="Corn tortillas"; qty=8; unit="pcs"}) },
    @{ title = "Beef Satay"; desc = "Thai grilled beef skewers"; allergens = @(5, 6); steps = @("Marinate beef", "Thread on skewers", "Grill", "Serve with peanut sauce"); ingredients = @(@{name="Beef sirloin"; qty=800; unit="g"}, @{name="Peanut butter"; qty=100; unit="g"}) },
    @{ title = "Vegetable Tempura"; desc = "Japanese battered vegetables"; allergens = @(1, 3); steps = @("Make batter", "Dip vegetables", "Fry", "Serve with dipping sauce"); ingredients = @(@{name="Mixed vegetables"; qty=500; unit="g"}, @{name="Flour"; qty=100; unit="g"}) },
    @{ title = "Chicken Tikka"; desc = "Indian marinated grilled chicken"; allergens = @(7); steps = @("Marinate chicken", "Grill on skewers", "Rest", "Serve with chutney"); ingredients = @(@{name="Chicken breast"; qty=800; unit="g"}, @{name="Yogurt"; qty=200; unit="ml"}) },
    @{ title = "Fish Tacos"; desc = "Crispy fish tacos with slaw"; allergens = @(1, 4); steps = @("Fry fish", "Make slaw", "Warm tortillas", "Assemble"); ingredients = @(@{name="White fish"; qty=500; unit="g"}, @{name="Flour tortillas"; qty=8; unit="pcs"}) },
    @{ title = "Minestrone Soup"; desc = "Italian vegetable and bean soup"; allergens = @(1, 6); steps = @("Sauté base", "Add vegetables", "Add broth", "Simmer"); ingredients = @(@{name="Mixed vegetables"; qty=800; unit="g"}, @{name="Borlotti beans"; qty=200; unit="g"}) },
    @{ title = "Grilled Vegetables"; desc = "Char-grilled mixed vegetables"; allergens = @(9); steps = @("Oil vegetables", "Grill", "Season", "Serve"); ingredients = @(@{name="Mixed vegetables"; qty=800; unit="g"}, @{name="Olive oil"; qty=100; unit="ml"}) },
    @{ title = "Pork Chops"; desc = "Pan-seared pork chops"; allergens = @(); steps = @("Season pork", "Sear in pan", "Finish in oven", "Rest before serving"); ingredients = @(@{name="Pork chops"; qty=4; unit="pcs"}, @{name="Garlic"; qty=4; unit="cloves"}) },
    @{ title = "Beet Salad"; desc = "Roasted beet salad"; allergens = @(7); steps = @("Roast beets", "Cool and slice", "Mix greens", "Add dressing"); ingredients = @(@{name="Beets"; qty=600; unit="g"}, @{name="Goat cheese"; qty=100; unit="g"}) },
    @{ title = "Chicken Fajitas"; desc = "Sautéed chicken with peppers"; allergens = @(); steps = @("Marinate chicken", "Slice and sauté", "Add vegetables", "Serve with tortillas"); ingredients = @(@{name="Chicken breast"; qty=800; unit="g"}, @{name="Bell peppers"; qty=400; unit="g"}) },
    @{ title = "Whole Grain Salad"; desc = "Quinoa and vegetable salad"; allergens = @(); steps = @("Cook quinoa", "Cool", "Mix vegetables", "Add dressing"); ingredients = @(@{name="Quinoa"; qty=200; unit="g"}, @{name="Mixed vegetables"; qty=400; unit="g"}) },
    @{ title = "Shrimp Fried Rice"; desc = "Asian shrimp and rice"; allergens = @(2, 6); steps = @("Cook rice ahead", "Fry rice", "Add shrimp", "Add soy sauce"); ingredients = @(@{name="Shrimp"; qty=300; unit="g"}, @{name="Rice"; qty=300; unit="g"}) },
    @{ title = "Vegetable Crepes"; desc = "French crepes with vegetables"; allergens = @(1, 3, 7); steps = @("Make batter", "Cook crepes", "Sauté vegetables", "Fill and roll"); ingredients = @(@{name="Flour"; qty=100; unit="g"}, @{name="Eggs"; qty=2; unit="pcs"}) },
    @{ title = "Beef Meatballs"; desc = "Italian-style meatballs"; allergens = @(1, 3); steps = @("Mix beef", "Form balls", "Reduce heat", "Simmer in sauce"); ingredients = @(@{name="Ground beef"; qty=600; unit="g"}, @{name="Breadcrumbs"; qty=100; unit="g"}) },
    @{ title = "Greek Yogurt Parfait"; desc = "Yogurt with granola and fruit"; allergens = @(7, 8); steps = @("Layer yogurt", "Add granola", "Add fruit", "Serve"); ingredients = @(@{name="Greek yogurt"; qty=300; unit="g"}, @{name="Granola"; qty=100; unit="g"}) },
    @{ title = "Vegetable Chili"; desc = "Bean and vegetable chili"; allergens = @(6); steps = @("Sauté vegetables", "Add beans", "Add spices", "Simmer"); ingredients = @(@{name="Mixed beans"; qty=600; unit="g"}, @{name="Tomatoes"; qty=400; unit="g"}) },
    @{ title = "Lobster Mac and Cheese"; desc = "Lobster in creamy pasta"; allergens = @(1, 2, 7); steps = @("Cook pasta", "Make cheese sauce", "Add lobster", "Serve"); ingredients = @(@{name="Pasta"; qty=400; unit="g"}, @{name="Lobster"; qty=300; unit="g"}) },
    @{ title = "Chicken Pot Pie"; desc = "Creamy chicken pie"; allergens = @(1, 3, 7); steps = @("Make filling", "Layer in dish", "Top with pastry", "Bake"); ingredients = @(@{name="Chicken"; qty=600; unit="g"}, @{name="Puff pastry"; qty=300; unit="g"}) },
    @{ title = "Mediterranean Wrap"; desc = "Fresh Mediterranean in wrap"; allergens = @(1, 7, 9); steps = @("Layer ingredients", "Add hummus", "Roll tightly", "Serve"); ingredients = @(@{name="Wrap"; qty=2; unit="pcs"}, @{name="Hummus"; qty=100; unit="g"}) },
    @{ title = "Beef Goulash"; desc = "Hungarian spiced beef stew"; allergens = @(); steps = @("Brown beef", "Add paprika", "Add vegetables", "Simmer"); ingredients = @(@{name="Beef chuck"; qty=1000; unit="g"}, @{name="Paprika"; qty=3; unit="tbsp"}) },
    @{ title = "Prawn Curry"; desc = "Creamy prawn curry"; allergens = @(2); steps = @("Make curry paste", "Add cream", "Add prawns", "Simmer"); ingredients = @(@{name="Prawns"; qty=600; unit="g"}, @{name="Coconut cream"; qty=300; unit="ml"}) },
    @{ title = "Vegetable Strudel"; desc = "Pastry-wrapped vegetables"; allergens = @(1, 7); steps = @("Make filling", "Wrap in pastry", "Brush with butter", "Bake"); ingredients = @(@{name="Mixed vegetables"; qty=600; unit="g"}, @{name="Phyllo dough"; qty=200; unit="g"}) },
    @{ title = "Chicken Schnitzel"; desc = "Fried breaded chicken cutlet"; allergens = @(1, 3); steps = @("Pound chicken", "Bread", "Fry", "Serve with lemon"); ingredients = @(@{name="Chicken breast"; qty=800; unit="g"}, @{name="Breadcrumbs"; qty=100; unit="g"}) },
    @{ title = "Beef Tacos Suizos"; desc = "Cheese-smothered beef tacos"; allergens = @(1, 7); steps = @("Cook beef", "Warm tortillas", "Fill and roll", "Top with cheese and sauce"); ingredients = @(@{name="Ground beef"; qty=500; unit="g"}, @{name="Corn tortillas"; qty=8; unit="pcs"}) },
    @{ title = "Vegetable Tart"; desc = "Tart with roasted vegetables"; allergens = @(1, 3, 7); steps = @("Make pastry", "Roast vegetables", "Layer in tart", "Bake"); ingredients = @(@{name="Pastry"; qty=300; unit="g"}, @{name="Mixed vegetables"; qty=600; unit="g"}) },
    @{ title = "Chicken Milanese"; desc = "Breaded and fried chicken"; allergens = @(1, 3); steps = @("Pound chicken", "Bread", "Fry", "Serve with lemon"); ingredients = @(@{name="Chicken breast"; qty=800; unit="g"}, @{name="Panko breadcrumbs"; qty=100; unit="g"}) },
    @{ title = "Black Cod Miso"; desc = "Miso-glazed black cod"; allergens = @(4, 6); steps = @("Mix miso glaze", "Marinate fish", "Grill or bake", "Serve"); ingredients = @(@{name="Black cod"; qty=600; unit="g"}, @{name="Miso paste"; qty=50; unit="g"}) },
    @{ title = "Chickpea Salad"; desc = "Cold chickpea protein salad"; allergens = @(9); steps = @("Cook chickpeas", "Cool", "Mix with vegetables", "Add dressing"); ingredients = @(@{name="Canned chickpeas"; qty=400; unit="g"}, @{name="Cherry tomatoes"; qty=200; unit="g"}) },
    @{ title = "Pork Schnitzel"; desc = "Fried breaded pork cutlet"; allergens = @(1, 3); steps = @("Pound pork", "Bread", "Fry", "Serve hot"); ingredients = @(@{name="Pork chops"; qty=4; unit="pcs"}, @{name="Breadcrumbs"; qty=100; unit="g"}) },
    @{ title = "Vegetable Cakes"; desc = "Pan-fried vegetable patties"; allergens = @(1, 3); steps = @("Mix vegetables", "Form patties", "Pan-fry", "Serve"); ingredients = @(@{name="Mixed vegetables"; qty=600; unit="g"}, @{name="Breadcrumbs"; qty=100; unit="g"}) },
    @{ title = "Chicken Meatballs"; desc = "Asian chicken meatballs"; allergens = @(1, 3, 6); steps = @("Mix chicken", "Form balls", "Bake or fry", "Add to sauce"); ingredients = @(@{name="Ground chicken"; qty=600; unit="g"}, @{name="Breadcrumbs"; qty=100; unit="g"}) },
    @{ title = "Beef Lettuce Wraps"; desc = "Asian beef in lettuce leaves"; allergens = @(6); steps = @("Dice beef", "Stir-fry", "Add sauce", "Serve in lettuce"); ingredients = @(@{name="Ground beef"; qty=500; unit="g"}, @{name="Butter lettuce"; qty=200; unit="g"}) },
    @{ title = "Vegetable Fritters"; desc = "Fried vegetable snacks"; allergens = @(1, 3); steps = @("Grate vegetables", "Mix batter", "Fry", "Drain on paper"); ingredients = @(@{name="Zucchini"; qty=300; unit="g"}, @{name="Flour"; qty=100; unit="g"}) },
    @{ title = "Beef Chili Con Carne"; desc = "Spiced beef and bean chili"; allergens = @(6); steps = @("Brown beef", "Add beans", "Add spices", "Simmer long"); ingredients = @(@{name="Ground beef"; qty=600; unit="g"}, @{name="Beans"; qty=400; unit="g"}) },
    @{ title = "Roasted Chicken"; desc = "Whole roasted chicken"; allergens = @(); steps = @("Season chicken", "Roast", "Rest", "Carve and serve"); ingredients = @(@{name="Whole chicken"; qty=1500; unit="g"}, @{name="Herbs"; qty=50; unit="g"}) },
    @{ title = "Vegetable Kebabs"; desc = "Grilled vegetable skewers"; allergens = @(9); steps = @("Cut vegetables", "Oil and season", "Thread on skewers", "Grill"); ingredients = @(@{name="Mixed vegetables"; qty=600; unit="g"}, @{name="Olive oil"; qty=50; unit="ml"}) },
    @{ title = "Pork Adobo"; desc = "Philippine pork stew"; allergens = @(6); steps = @("Brown pork", "Add vinegar", "Add soy sauce", "Simmer"); ingredients = @(@{name="Pork shoulder"; qty=800; unit="g"}, @{name="Soy sauce"; qty=100; unit="ml"}) },
    @{ title = "Calamari Fritti"; desc = "Fried squid rings"; allergens = @(1, 3, 10); steps = @("Clean squid", "Slice into rings", "Bread and fry", "Serve with sauce"); ingredients = @(@{name="Squid"; qty=600; unit="g"}, @{name="Flour"; qty=100; unit="g"}) },
    @{ title = "Vegetable Pilaf"; desc = "Spiced rice with vegetables"; allergens = @(); steps = @("Toast rice", "Sauté vegetables", "Add broth", "Simmer"); ingredients = @(@{name="Basmati rice"; qty=300; unit="g"}, @{name="Mixed vegetables"; qty=300; unit="g"}) },
    @{ title = "Duck With Orange"; desc = "Duck in orange sauce"; allergens = @(); steps = @("Roast duck", "Make orange sauce", "Reduce sauce", "Serve"); ingredients = @(@{name="Duck breast"; qty=600; unit="g"}, @{name="Oranges"; qty=2; unit="pcs"}) },
    @{ title = "Eggplant Parmesan"; desc = "Fried eggplant with cheese"; allergens = @(1, 3, 7); steps = @("Slice and fry eggplant", "Layer with sauce", "Top with cheese", "Bake"); ingredients = @(@{name="Eggplant"; qty=500; unit="g"}, @{name="Parmesan"; qty=200; unit="g"}) },
    @{ title = "Turkey Meatloaf"; desc = "Ground turkey meatloaf"; allergens = @(1, 3); steps = @("Mix turkey", "Form loaf", "Bake", "Rest before serving"); ingredients = @(@{name="Ground turkey"; qty=800; unit="g"}, @{name="Breadcrumbs"; qty=100; unit="g"}) },
    @{ title = "Coconut Rice"; desc = "Creamy coconut rice"; allergens = @(); steps = @("Toast rice", "Add water and coconut milk", "Simmer", "Fluff"); ingredients = @(@{name="Jasmine rice"; qty=300; unit="g"}, @{name="Coconut milk"; qty=400; unit="ml"}) },
    @{ title = "Beef Stroganoff"; desc = "Creamy beef over noodles"; allergens = @(1, 7); steps = @("Brown beef", "Make sauce with sour cream", "Combine", "Serve over noodles"); ingredients = @(@{name="Beef sirloin"; qty=800; unit="g"}, @{name="Sour cream"; qty=200; unit="ml"}) },
    @{ title = "Spinach and Feta Pie"; desc = "Greek spinach pie"; allergens = @(1, 7); steps = @("Sauté spinach", "Mix with cheese", "Layer in phyllo", "Bake"); ingredients = @(@{name="Spinach"; qty=500; unit="g"}, @{name="Feta cheese"; qty=200; unit="g"}) },
    @{ title = "Chicken Gumbo"; desc = "Louisiana chicken stew"; allergens = @(); steps = @("Make roux", "Add vegetables", "Add chicken", "Simmer"); ingredients = @(@{name="Chicken"; qty=800; unit="g"}, @{name="Okra"; qty=300; unit="g"}) },
    @{ title = "Baked Cod"; desc = "Herb-baked white fish"; allergens = @(1, 4); steps = @("Season cod", "Bake with herbs", "Finish at high heat", "Serve"); ingredients = @(@{name="Cod fillets"; qty=600; unit="g"}, @{name="Herbs"; qty=30; unit="g"}) },
    @{ title = "Lentil Soup"; desc = "Hearty lentil soup"; allergens = @(); steps = @("Sauté base", "Add lentils", "Add broth", "Simmer"); ingredients = @(@{name="Lentils"; qty=300; unit="g"}, @{name="Vegetables"; qty=400; unit="g"}) },
    @{ title = "Chicken Jambalaya"; desc = "Rice with chicken and sausage"; allergens = @(); steps = @("Cook sausage", "Add vegetables", "Add rice", "Simmer with broth"); ingredients = @(@{name="Chicken"; qty=600; unit="g"}, @{name="Sausage"; qty=300; unit="g"}) },
    @{ title = "Vegetable Soup"; desc = "Light veggie soup"; allergens = @(); steps = @("Sauté base", "Add vegetables", "Add broth", "Simmer gently"); ingredients = @(@{name="Mixed vegetables"; qty=800; unit="g"}, @{name="Vegetable broth"; qty=1000; unit="ml"}) },
    @{ title = "Pork Carnitas"; desc = "Slow-cooked pork"; allergens = @(); steps = @("Season pork", "Slow cook", "Shred meat", "Serve in tortillas"); ingredients = @(@{name="Pork shoulder"; qty=1200; unit="g"}, @{name="Garlic"; qty=8; unit="cloves"}) },
    @{ title = "Capellini Aglio e Olio"; desc = "Garlic and oil pasta"; allergens = @(1, 9); steps = @("Cook pasta", "Infuse oil", "Combine quickly", "Serve hot"); ingredients = @(@{name="Capellini"; qty=400; unit="g"}, @{name="Garlic"; qty=10; unit="cloves"}) },
    @{ title = "Fish Ceviche"; desc = "Raw fish in lime juice"; allergens = @(4); steps = @("Dice fish", "Cover with lime", "Marinate 1 hour", "Add vegetables"); ingredients = @(@{name="Mahi mahi"; qty=600; unit="g"}, @{name="Limes"; qty=6; unit="pcs"}) },
    @{ title = "Vegetable Tapenade"; desc = "Olive and vegetable spread"; allergens = @(9); steps = @("Blend olives", "Add capers", "Mix vegetables", "Serve with bread"); ingredients = @(@{name="Olives"; qty=300; unit="g"}, @{name="Capers"; qty=50; unit="g"}) },
    @{ title = "Thai Red Curry"; desc = "Spicy red curry"; allergens = @(2); steps = @("Fry curry paste", "Add coconut milk", "Add meat", "Simmer"); ingredients = @(@{name="Coconut milk"; qty=400; unit="ml"}, @{name="Red curry paste"; qty=3; unit="tbsp"}) },
    @{ title = "Gazpacho"; desc = "Cold Spanish tomato soup"; allergens = @(1); steps = @("Blend vegetables", "Chill", "Season to taste", "Serve cold"); ingredients = @(@{name="Tomatoes"; qty=800; unit="g"}, @{name="Bread"; qty=100; unit="g"}) },
    @{ title = "Lamb Chops"; desc = "Grilled lamb chops"; allergens = @(9); steps = @("Marinate lamb", "Grill", "Rest briefly", "Serve with sauce"); ingredients = @(@{name="Lamb chops"; qty=8; unit="pcs"}, @{name="Olive oil"; qty=50; unit="ml"}) },
    @{ title = "Mushroom Tart"; desc = "Savory mushroom puff tart"; allergens = @(1, 3, 7); steps = @("Saute mushrooms", "Layer on puff pastry", "Top with cheese", "Bake"); ingredients = @(@{name="Mushrooms"; qty=600; unit="g"}, @{name="Puff pastry"; qty=300; unit="g"}) },
    @{ title = "Chicken Quesadilla"; desc = "Grilled chicken and cheese"; allergens = @(1, 7); steps = @("Cook chicken", "Layer ingredients", "Griddle until crispy", "Cut and serve"); ingredients = @(@{name="Chicken breast"; qty=300; unit="g"}, @{name="Flour tortillas"; qty=4; unit="pcs"}) },
    @{ title = "Roasted Vegetables"; desc = "Oven-roasted seasonal veg"; allergens = @(9); steps = @("Cut vegetables", "Toss with oil", "Roast", "Season after"); ingredients = @(@{name="Mixed vegetables"; qty=1000; unit="g"}, @{name="Olive oil"; qty=100; unit="ml"}) }
)

# Create 100 unique recipes (some duplicates with different spins)
for ($i = 1; $i -le 100; $i++) {
    $recipe = $recipes[($i - 1) % 100]
    
    # Create ingredient JSON
    $ingredientsJson = @()
    foreach ($ing in $recipe.ingredients) {
        $ingredientsJson += @{
            name = $ing.name
            quantity = $ing.qty
            unitOfMeasurement = $ing.unit
        }
    }
    
    # Create the request body
    $bodyContent = @{
        title = "$($recipe.title) ($i)"
        description = $recipe.desc
        allergens = $recipe.allergens
        steps = $recipe.steps
        ingredients = $ingredientsJson
    } | ConvertTo-Json -Depth 10
    
    # Create the Bruno request file
    $seq = $i + 2
    $filename = "recipe_$($i.ToString('D3')).yml"
    $filepath = Join-Path $recipeDir $filename
    
    $content = @"
info:
  name: recipe $i
  type: http
  seq: $seq

http:
  method: POST
  url: http://localhost:9393/api/recipes/
  body:
    type: json
    data: |-
      $bodyContent
  auth: inherit

settings:
  encodeUrl: true
  timeout: 0
  followRedirects: true
  maxRedirects: 5
"@
    
    Set-Content -Path $filepath -Value $content -Encoding UTF8
    Write-Host "Created: $filename"
}

Write-Host "`nSuccessfully generated 100 recipe API calls in $recipeDir"
