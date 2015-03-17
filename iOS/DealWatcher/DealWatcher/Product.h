//
//  Product.h
//  DealWatcher
//
//  Created by Dylan Sturgeon on 3/17/15.
//  Copyright (c) 2015 dylansturg. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreData/CoreData.h>

@class ProductCode, ProductImage, ProductPrice;

@interface Product : NSManagedObject

@property (nonatomic, retain) NSNumber * productId;
@property (nonatomic, retain) NSString * displayName;
@property (nonatomic, retain) NSSet *productImages;
@property (nonatomic, retain) NSSet *productCodes;
@property (nonatomic, retain) NSSet *productPrices;
@end

@interface Product (CoreDataGeneratedAccessors)

- (void)addProductImagesObject:(ProductImage *)value;
- (void)removeProductImagesObject:(ProductImage *)value;
- (void)addProductImages:(NSSet *)values;
- (void)removeProductImages:(NSSet *)values;

- (void)addProductCodesObject:(ProductCode *)value;
- (void)removeProductCodesObject:(ProductCode *)value;
- (void)addProductCodes:(NSSet *)values;
- (void)removeProductCodes:(NSSet *)values;

- (void)addProductPricesObject:(ProductPrice *)value;
- (void)removeProductPricesObject:(ProductPrice *)value;
- (void)addProductPrices:(NSSet *)values;
- (void)removeProductPrices:(NSSet *)values;

@end
