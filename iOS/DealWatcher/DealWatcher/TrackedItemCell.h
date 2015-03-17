//
//  TrackedItemCell.h
//  DealWatcher
//
//  Created by Dylan Sturgeon on 3/16/15.
//  Copyright (c) 2015 dylansturg. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface TrackedItemCell : UITableViewCell
@property (weak, nonatomic) IBOutlet UIImageView *image;
@property (weak, nonatomic) IBOutlet UILabel *title;
@property (weak, nonatomic) IBOutlet NSLayoutConstraint *subtitle;

@end
