//
//  TrackedItemsViewController.m
//  DealWatcher
//
//  Created by Dylan Sturgeon on 3/16/15.
//  Copyright (c) 2015 dylansturg. All rights reserved.
//

#import "TrackedItemsViewController.h"
#import "Product.h"
#import "ProductImage.h"
#import "TrackedItemCell.h"

#import <SDWebImage/UIImageView+WebCache.h>
#import <RestKit.h>

@interface TrackedItemsViewController ()
@property (nonatomic, strong) NSArray* products;
@property (nonatomic, copy) NSString* cellIdentifier;
@end

@implementation TrackedItemsViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    
    // Uncomment the following line to preserve selection between presentations.
    // self.clearsSelectionOnViewWillAppear = NO;
    
    // Uncomment the following line to display an Edit button in the navigation bar for this view controller.
    // self.navigationItem.rightBarButtonItem = self.editButtonItem;
    _cellIdentifier = @"TrackedItemCell";
    
    [self tableView].rowHeight = 60.0f;
    
    //[self loadTrackedItems];
    [self fetchTrackedItemsFromContext];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

#pragma mark - Table view data source

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
    // Return the number of rows in the section.
    return [self.products count];
}


- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    
    TrackedItemCell *cell = [tableView dequeueReusableCellWithIdentifier:_cellIdentifier forIndexPath:indexPath];
    
    // Configure the cell...
    NSInteger index = indexPath.row;
    if ([self.products count] > index) {
        Product *product = self.products[index];
        cell.title.text = product.displayName;
        if (product.productImages.count > 0) {
            ProductImage *image = (ProductImage*)[product.productImages anyObject];
            NSString* imageUrl = image.url;
            [cell.productImage sd_setImageWithURL:[NSURL URLWithString:imageUrl]];
        }

        
    }
    
    return cell;
}

- (void)loadTrackedItems{
    NSString *requestPath = @"/api/Products";
    NSString *bearerToken = @"fDb478CPvt_MC5wd5DnlqORDCn6QxgSeezKAw2n7megh6qOTWP-BF9r2RYTXKMqeQTaViPvrYxROwq0SZUd50X9f-mRwCfNrK77YSSOgmxvKoZn6tETpxl9FieGw01rykp6tWEMdN-g9VL8ypIF_7tB9ViuWecltq_-FQzq0Ur4hN26DQpNXlnkpWV1Lr_ivHeelSqIYQFCsfgG0n0dVf8X0eGYURipeWrxC-0_H9CPsL0adya2d-ovHL5Ffs3nzvg5LMIQEkkti8rCKAq6vOTkUuUqWEJYtP3M_5Ja86iia5x5MpMduywKrwP15dvli8sD0dFlsfH9zB-WpyqdBIVd_G0QMcS-8cg07IGTuxDGngUeeC1GyCXi6MW--qeIQ7nh81dIlU_IT8e8zcKZC8oaSxNuevN5UVSWVKkWif1FjQf79AtAEbJah497dZhHkgHRbreZFt-08LjH4GlXlbngevlMLjUv7ND_a5s1-fLO5jiXUYYMnw-I1_0uImwsn";
    
    RKObjectManager *objectManager = [RKObjectManager sharedManager];
    [objectManager.HTTPClient setDefaultHeader:@"Authorization" value:[NSString stringWithFormat:@"Bearer %@", bearerToken]];
    
    [objectManager getObjectsAtPath:requestPath parameters:nil
                            success: ^(RKObjectRequestOperation *operation, RKMappingResult *mappingResult) {
                                [self fetchTrackedItemsFromContext];
                            }
                            failure: ^(RKObjectRequestOperation *operation, NSError *error) {
                                RKLogError(@"Load failed with error: %@", error);
                            }];
}

- (void)fetchTrackedItemsFromContext {
    NSManagedObjectContext *context = [RKManagedObjectStore defaultStore].mainQueueManagedObjectContext;
    NSFetchRequest *fetchRequest = [NSFetchRequest fetchRequestWithEntityName:@"Product"];
    
    NSSortDescriptor *descriptor = [NSSortDescriptor sortDescriptorWithKey:@"productId" ascending:YES];
    fetchRequest.sortDescriptors = @[descriptor];
    
    NSError *error = nil;
    NSArray *fetchedObjects = [context executeFetchRequest:fetchRequest error:&error];
    
    self.products = fetchedObjects;
    
    [self.tableView reloadData];
}

/*
#pragma mark - Navigation

// In a storyboard-based application, you will often want to do a little preparation before navigation
- (void)prepareForSegue:(UIStoryboardSegue *)segue sender:(id)sender {
    // Get the new view controller using [segue destinationViewController].
    // Pass the selected object to the new view controller.
}
*/

@end
